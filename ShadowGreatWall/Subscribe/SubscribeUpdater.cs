using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowGreatWall.Core;
using ShadowGreatWall.Startup;
using Org.Core.Log;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using ShadowGreatWall.Encryption;

namespace ShadowGreatWall.Subscribe
{
    class SubscribeUpdater
    {
        #region [委托]
        private delegate IServer ParseProtocolDelegate(string protocol);
        #endregion

        #region [变量]
        private Configuration config = StartupMgr.Instance.CurrentConfig;

        private Dictionary<string, ParseProtocolDelegate> parseMethods = new Dictionary<string, ParseProtocolDelegate>();
        #endregion

        public SubscribeUpdater()
        {
            parseMethods.Add("ss://", new ParseProtocolDelegate(ParseSSProtocol));
            parseMethods.Add("ssr://", new ParseProtocolDelegate(ParseSSRProtocol));
        }

        public void Update()
        {
            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("========== 更新订阅 at {0} ==========", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));

            foreach (ServerGroup group in config.Groups)
            {
                Update(group);
            }

            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("========== 更新订阅完成 at {0} ==========", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
        }

        private void Update(ServerGroup group)
        {
            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("********** 开始更新 {0} **********", group.Name));
            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>("基本信息:");
            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("URL: {0}", group.URL));
            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("GUID: {0}", group.Guid));

            try
            {
                WebClient http = new WebClient();

                if (!config.Proxy.Enable || config.Proxy.UpdateVia)
                {
                    http.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.3319.102 Safari/537.36");
                    http.Proxy = null;
                }
                else
                {
                    http.Headers.Add("User-Agent",
                    String.IsNullOrEmpty(config.Proxy.UserAgent) ?
                    "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.3319.102 Safari/537.36"
                    : config.Proxy.UserAgent);

                    WebProxy proxy = new WebProxy(IPAddress.Loopback.ToString(), config.Proxy.Port);

                    if (!string.IsNullOrEmpty(config.Proxy.Password))
                    {
                        proxy.Credentials = new NetworkCredential(config.Proxy.Accout, config.Proxy.Password);
                    }

                    http.Proxy = proxy;
                }
                
                http.QueryString["rnd"] = Guid.NewGuid().ToString();

                string URL = group.URL;

                //add support for tls1.2+
                if (URL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072 | SecurityProtocolType.Tls;
                }

                string response = http.DownloadString(new Uri(URL));

                ParseResponse(group, response);

                AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("********** 结束更新 {0} **********", group.Name));

                NotificationMgr.Instance.TryNotify(NotifycationType.SubscribeUpdated);
            }
            catch (Exception e)
            {
                AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(e);
            }

            Configuration.Save(config);
        }

        private void ParseResponse(ServerGroup group, string response)
        {
            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("从服务器获取的返回值: {0}", response));

            response = response.TrimEnd('\r', '\n', ' ');

            try
            {
                response = Encoding.UTF8.GetString(Convert.FromBase64String(response.PadRight(response.Length + (4 - response.Length % 4) % 4, '=')));
            }
            catch (Exception ex)
            {
                AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(ex);
            }

            AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("解码出来的返回值: {0}", response));

            List<IServer> servers = new List<IServer>();

            #region [逐行解析]
            using (StringReader reader = new StringReader(response))
            {
                string protocol = null;

                while ((protocol = reader.ReadLine()) != null)
                {
                    protocol = protocol.Trim();

                    foreach (KeyValuePair<string, ParseProtocolDelegate> pair in parseMethods)
                    {
                        if (protocol.StartsWith(pair.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                protocol = protocol.Substring(pair.Key.Length);
                                protocol = Encoding.UTF8.GetString(Convert.FromBase64String(protocol.PadRight(protocol.Length + (4 - protocol.Length % 4) % 4, '=')));

                                IServer server = pair.Value.Invoke(protocol);

                                if (server == null)
                                {
                                    break;
                                }
                                else
                                {
                                    protocol = null;
                                    server.GroupGuid = group.Guid;
                                    servers.Add(server);
                                }

                                break;
                            }
                            catch (Exception ex)
                            {
                                AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(ex);
                                break;
                            }
                        }
                    }

                    if (protocol != null)
                    {
                        AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("无法解析服务器信息: {0}", protocol));
                    }
                }
            }
            #endregion

            #region [结果校验]
            //检查结果集是否存在已有的Server，有的话把Server复制过来
            for (int i = 0; i < servers.Count; i++)
            {
                IServer server = servers[i];

                foreach (IServer oldServer in config.Servers)
                {
                    if (oldServer.EqualTo(server))
                    {
                        config.Servers.Remove(oldServer);

                        oldServer.Copy(server);
                        oldServer.GroupGuid = group.Guid;
                        servers[i] = oldServer;
                        break;
                    }
                }
            }

            config.Servers = servers;
            #endregion
        }

        private IServer ParseSSProtocol(string protocol)
        {
            Regex regex = new Regex(@"^(.+?):(.+?)@(.+?):((?:[0-9]{1}|[1-9]\d{1,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])+?)$");

            var match = regex.Match(protocol);

            if (!match.Success)
            {
                AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("该SS订阅信息无法解析, Protocol: {0}", protocol));
                return null;
            }

            string algorithm = match.Groups[1].Value;
            string password = match.Groups[2].Value;
            string host = match.Groups[3].Value;
            int port = Convert.ToInt32(match.Groups[4].Value);

            if (!EncryptionCenter.IsSupport(algorithm))
            {
                AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("该SS加密方案无法支持, Protocol: {0}", protocol));
                return null;
            }

            ShadowsockServer server = new ShadowsockServer();
            server.Protocol.Name = "origin";
            server.Algorithm = algorithm;
            server.Host = host;
            server.Port = port;

            return server;
        }

        private IServer ParseSSRProtocol(string protocol)
        {
            string param = "";

            {
                int paramSplit = protocol.IndexOf("/?");

                if (paramSplit >= 0)
                {
                    param = protocol.Substring(paramSplit + 2);
                    protocol = protocol.Substring(0, paramSplit);
                }

                string[] baseParams = protocol.Split(':');

                if (baseParams.Length != 6)
                {
                    AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("该SSR订阅信息无法解析，因为参数个数不对(需要6个), Protocol: {0}", protocol));
                    return null;
                }

                try
                {
                    ShadowsockServer server = new ShadowsockServer();
                    server.Host = baseParams[0];
                    server.Port = Convert.ToInt32(baseParams[1]);
                    server.Protocol.Name = (string.IsNullOrWhiteSpace(baseParams[2]) ? "origin" : baseParams[2]).Replace("_compatible", "");
                    server.Algorithm = baseParams[3];
                    server.Obscure.Name = (string.IsNullOrWhiteSpace(baseParams[4]) ? "plain" : baseParams[4]).Replace("_compatible", "");
                    server.Password = Utils.FrmUrlSafeBase64(baseParams[5]);

                    if (!string.IsNullOrWhiteSpace(param))
                    {
                        ParseSSRParameters(server, param);
                    }

                    return server;
                }
                catch (Exception ex)
                {
                    AppLogProxy.AppLog.WriteLog<SubscribeUpdater>(string.Format("该SSR订阅信息无法解析，因为参数类型不正确, Protocol: {0}", protocol), ex);
                    return null;
                }
            }
        }

        private void ParseSSRParameters(ShadowsockServer server, string data)
        {
            string[] param = data.Split('&');

            foreach (string paramData in param)
            {
                if (string.IsNullOrWhiteSpace(paramData))
                {
                    continue;
                }

                int split = paramData.IndexOf('=');

                if (split < 1)
                {
                    continue;
                }

                string key = paramData.Substring(0, split).ToLower();
                string value = paramData.Substring(split + 1);

                switch (key)
                { 
                    case "protoparam":
                        server.Protocol.Parameter = Utils.FrmUrlSafeBase64(value);
                        break;
                    case "obfsparam":
                        server.Obscure.Parameter = Utils.FrmUrlSafeBase64(value);
                        break;
                    case "remarks":
                        server.Describe = Utils.FrmUrlSafeBase64(value);
                        break;
                }
            }
        }
    }
}
