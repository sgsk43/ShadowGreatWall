using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Org.Core.Log;
using System.Reflection;

namespace ShadowGreatWall.Core
{
    public class Configuration
    {
        #region [变量]
        private const string CONFIGFILE = @"server_config.json";
        #endregion

        #region [加载]
        public static Configuration Load()
        {
            try
            {
                string configContent = File.ReadAllText(CONFIGFILE);
                Configuration ret = Load(configContent);

                if (ret == null)
                {
                    throw new Exception("配置文件加载失败，请检查加载器日志");
                }

                return ret;
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException))
                {
                    Console.WriteLine(e);

                    return new Configuration();
                }
                else
                {
                    return new Configuration();
                }
            }
        }

        public static void Save(Configuration config)
        {
            try
            {
                string content = SerializerMgr.Json.Serialize(config);

                if (string.IsNullOrWhiteSpace(content))
                {
                    return;
                }

                using (StreamWriter sw = new StreamWriter(File.Open(CONFIGFILE, FileMode.Create)))
                {
                    sw.Write(content);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                AppLogProxy.AppLog.WriteLog<Configuration>(e);
            }
        }

        private static Configuration Load(string content)
        {
            try
            {
                return SerializerMgr.Json.Desrialize<Configuration>(content);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region [接口]
        private Proxy proxy = new Proxy();

        public Proxy Proxy
        {
            get
            {
                return proxy;
            }
            set
            {
                if (value == null)
                {
                    proxy = new Proxy();
                }
                else
                {
                    proxy = value;
                }
            }
        }

        private readonly List<ServerGroup> groups = new List<ServerGroup>();
        public List<ServerGroup> Groups
        {
            get
            {
                return groups;
            }
            set
            {
                groups.Clear();

                if (value != null && value.Count > 0)
                {
                    groups.AddRange(value);
                }
            }
        }

        private readonly List<IServer> servers = new List<IServer>();
        public List<IServer> Servers
        {
            get
            {
                return servers;
            }
            set
            {
                servers.Clear();

                if (value != null && value.Count > 0)
                {
                    servers.AddRange(value);
                }
            }
        }

        public bool Enable { get; set; }
        #endregion
    }

    public class Proxy
    {
        public bool Enable { get; set; }
        public ProxyMode Mode { get; set; }
        public bool UpdateVia { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Accout { get; set; }
        public string Password { get; set; }
        public string UserAgent { get; set; }
    }

    public class ServerGroup
    {
        public ServerGroup()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
        public string Guid { get; set; }
        public string URL { get; set; }
    }

    public enum ServerType
    {
        Shadowsocks,
        Vmess
    }

    public enum ProxyMode
    { 
        Socks5,
        Http
    }

    public abstract class IServer
    {
        public bool Selected { get; set; }
        public string GroupGuid { get; set; }
        public ServerType ServerType { get; protected set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Describe { get; set; }

        public abstract bool EqualTo(IServer other);

        public virtual void Copy(IServer other)
        {
            Type thisType = this.GetType();

            if (thisType != other.GetType())
            {
                throw new Exception("类型不匹配");
            }

            PropertyInfo[] properties = thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead || !property.CanWrite || property.Name == "Selected")
                {
                    continue;
                }

                property.SetValue(other, property.GetValue(this, null), null);
            }
        }
    }

    public class ServerParameter
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
    }

    public class ShadowsockServer : IServer
    {
        public ShadowsockServer()
        {
            this.ServerType = ServerType.Shadowsocks;
        }

        public string Password { get; set; }
        public string Algorithm { get; set; }   //加密

        private ServerParameter protocol = new ServerParameter();
        public ServerParameter Protocol     //协议
        {
            get
            {
                return protocol;
            }
            set
            {
                if (value == null)
                {
                    protocol = new ServerParameter();
                }
                else
                {
                    protocol = value;
                }
            }
        }

        private ServerParameter obscure = new ServerParameter();
        public ServerParameter Obscure     //混淆
        {
            get
            {
                return obscure;
            }
            set
            {
                if (value == null)
                {
                    obscure = new ServerParameter();
                }
                else
                {
                    obscure = value;
                }
            }
        }
        public object Plugin { get; set; }

        public override bool EqualTo(IServer other)
        {
            ShadowsockServer otherServer = other as ShadowsockServer;

            if (otherServer == null)
            {
                return false;
            }

            return string.Equals(this.Describe, otherServer.Describe);
        }
    }

    public class VmessServer : IServer
    {
        public VmessServer()
        {
            this.ServerType = ServerType.Vmess;
        }

        public string UUID { get; set; }
        public object Algorithm { get; set; }
        public bool TLS { get; set; }
        public object Confusion { get; set; }
        public bool Multiplexing { get; set; }

        public override bool EqualTo(IServer other)
        {
            VmessServer otherServer = other as VmessServer;

            if (otherServer == null)
            {
                return false;
            }

            return string.Equals(this.Describe, otherServer.Describe);
        }
    }
}
