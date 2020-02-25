using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Org.Core.Log;
using System.Reflection;
using System.Collections;

namespace ShadowGreatWall.Core.Serializer
{
    internal class CustomJsonSerializer : ISerializer
    {
        public override string Serialize(object Data)
        {
            try
            {
                JavaScriptSerializer oSer = new JavaScriptSerializer();
                oSer.RegisterConverters(new JavaScriptConverter[] {
                    new CloudDictionaryJsonConverter()
                });

                return oSer.Serialize(Data);
            }
            catch (Exception ex)
            {
                AppLogProxy.AppLog.WriteLog<CustomJsonSerializer>("序列化对象失败！", ex);

                return null;
            }
        }

        public override object Desrialize(Type Type, string Data)
        {
            try
            {
                JavaScriptSerializer oSer = new JavaScriptSerializer();
                oSer.RegisterConverters(new JavaScriptConverter[] {
                    new CloudDictionaryJsonConverter()
                });

                return oSer.Deserialize(Data, Type);
            }
            catch (Exception ex)
            {
                AppLogProxy.AppLog.WriteLog<CustomJsonSerializer>(string.Format("反序列化对象失败！\r\n{0}", Data), ex);

                return null;
            }
        }
    }

    internal class CloudDictionaryJsonConverter : JavaScriptConverter
    {
        #region [Serialize]
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Configuration data = obj as Configuration;

            if (data == null)
            {
                return null;
            }

            Type thisType = data.GetType();
            PropertyInfo[] properties = thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Dictionary<string, object> retData = new Dictionary<string, object>();

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead)
                {
                    continue;
                }

                if (property.Name == "Servers")
                {
                    SerializeServers(retData, data.Servers);
                }
                else if (property.Name == "Groups")
                {
                    SerializeGroups(retData, data.Groups);
                }
                else
                {
                    ReadPropertyValue(data, property, retData);
                }
            }

            //Dictionary<string, object> ret = new Dictionary<string, object>();
            //SerializeGroups(ret, data.Groups);
            //SerializeServers(ret, data.Servers);

            return retData;
        }

        private void SerializeGroups(Dictionary<string, object> data, List<ServerGroup> groups)
        {
            if (groups == null || groups.Count == 0)
            {
                return;
            }

            List<Dictionary<string, object>> groupDatas = new List<Dictionary<string, object>>();

            foreach (ServerGroup group in groups)
            {
                Type groupType = group.GetType();
                PropertyInfo[] properties = groupType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                Dictionary<string, object> groupData = new Dictionary<string, object>();

                foreach (PropertyInfo property in properties)
                {
                    ReadPropertyValue(group, property, groupData);
                }

                groupDatas.Add(groupData);
            }

            data.Add("Groups", groupDatas);
        }

        private void SerializeServers(Dictionary<string, object> data, List<IServer> servers)
        {
            if (servers == null || servers.Count == 0)
            {
                return;
            }

            List<Dictionary<string, object>> serverDatas = new List<Dictionary<string, object>>();

            foreach (IServer server in servers)
            {
                Type serverType = server.GetType();
                PropertyInfo[] properties = serverType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                Dictionary<string, object> serverData = new Dictionary<string, object>();

                foreach (PropertyInfo property in properties)
                {
                    ReadPropertyValue(server, property, serverData);
                }

                serverDatas.Add(serverData);
            }

            data.Add("Servers", serverDatas);
        }

        private void ReadPropertyValue(object obj, PropertyInfo property, Dictionary<string, object> data)
        {
            if (property.CanRead)
            {
                Type thisType = property.PropertyType;
                object value = property.GetValue(obj, null);

                if (thisType.IsEnum)
                {
                    data.Add(property.Name, value.ToString());
                }
                else if (thisType.IsPrimitive || thisType == typeof(string))
                {
                    data.Add(property.Name, value);
                }
                else
                {
                    Dictionary<string, object> retData = new Dictionary<string, object>();

                    PropertyInfo[] properties = thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    foreach (PropertyInfo subProperty in properties)
                    {
                        ReadPropertyValue(value, subProperty, retData);
                    }

                    data.Add(property.Name, retData);
                }
            }
        }
        #endregion

        #region [Deserialize]
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (type != typeof(Configuration))
            {
                return null;
            }

            Configuration config = new Configuration();
            object value = null;

            Type thisType = typeof(Configuration);
            PropertyInfo[] properties = thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Dictionary<string, object> retData = new Dictionary<string, object>();

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                if (!dictionary.TryGetValue(property.Name, out value))
                {
                    continue;
                }

                if (property.Name == "Servers")
                {
                    DeserializeServers(config, (IList)value);
                }
                else if (property.Name == "Groups")
                {
                    DeserializeGroups(config, (IList)value);
                }
                else
                {
                    WritePropertyValue(config, property, value);
                }
            }

            //if (dictionary.TryGetValue("Groups", out value))
            //{
            //    DeserializeGroups(config, (IList)value);
            //}

            //if (dictionary.TryGetValue("Servers", out value))
            //{
            //    DeserializeServers(config, (IList)value);
            //}

            return config;
        }

        private void DeserializeGroups(Configuration config, IList datas)
        {
            foreach (IDictionary data in datas)
            {
                ServerGroup group = new ServerGroup();
                Type groupType = group.GetType();
                PropertyInfo[] properties = groupType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    WritePropertyValue(group, property, data);
                }

                config.Groups.Add(group);
            }
        }

        private void DeserializeServers(Configuration config, IList datas)
        {
            foreach (IDictionary data in datas)
            {
                if (!data.Contains("ServerType"))
                {
                    continue;
                }

                ServerType type;

                if (!Enum.TryParse<ServerType>(data["ServerType"] as string, out type))
                {
                    continue;
                }

                IServer server = null;

                switch (type)
                {
                    case ServerType.Shadowsocks:
                        server = new ShadowsockServer();
                        break;
                    case ServerType.Vmess:
                        server = new VmessServer();
                        break;
                }

                if (server == null)
                {
                    AppLogProxy.AppLog.WriteLog<CustomJsonSerializer>(string.Format("无法识别的ServerType: {0}", type));
                    continue;
                }

                Type serverType = server.GetType();
                PropertyInfo[] properties = serverType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    WritePropertyValue(server, property, data);
                }

                config.Servers.Add(server);
            }
        }

        private void WritePropertyValue(object obj, PropertyInfo property, IDictionary data)
        {
            if (property.CanWrite)
            {
                if (data.Contains(property.Name))
                {
                    object value = data[property.Name];

                    WritePropertyValue(obj, property, value);
                }
            }
        }

        private void WritePropertyValue(object obj, PropertyInfo property, object data)
        {
            Type thisType = property.PropertyType;

            if (thisType.IsEnum)
            {
                property.SetValue(obj, Enum.Parse(property.PropertyType, (string)data), null);
            }
            else if (thisType.IsPrimitive || thisType == typeof(string))
            {
                property.SetValue(obj, data, null);
            }
            else
            {
                PropertyInfo[] properties = thisType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                object value = Activator.CreateInstance(thisType);

                foreach (PropertyInfo subProperty in properties)
                {
                    WritePropertyValue(value, subProperty, data as IDictionary);
                }

                property.SetValue(obj, value, null);
            }
        }
        #endregion

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new Type[] { typeof(Configuration) };
            }
        }
    }
}