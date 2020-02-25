using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Org.Core.Log;

namespace ShadowGreatWall.Core.Serializer
{
    internal class CustomXmlSerializer : ISerializer
    {
        public override string Serialize(object Data)
        {
            try
            {
                MemoryStream oMS = new MemoryStream();
                XmlSerializer oSer = new XmlSerializer(Data.GetType());
                oSer.Serialize(oMS, Data);

                oMS.Seek(0, SeekOrigin.Begin);

                StreamReader oReader = new StreamReader(oMS);
                string sRet = oReader.ReadToEnd();

                oReader.Dispose();
                oMS.Dispose();

                return sRet;
            }
            catch (Exception ex)
            {
                AppLogProxy.AppLog.WriteLog<CustomXmlSerializer>("序列化对象失败！", ex);

                return null;
            }
        }

        public override object Desrialize(Type Type, string Data)
        {
            try
            {
                StringReader oStringReader = new StringReader(Data);
                XmlReader oXmlReader = new XmlTextReader(oStringReader);

                XmlSerializer oSer = new XmlSerializer(Type);

                object oRet = oSer.Deserialize(oXmlReader);

                oXmlReader.Close();
                oStringReader.Dispose();

                return oRet;
            }
            catch (Exception ex)
            {
                AppLogProxy.AppLog.WriteLog<CustomXmlSerializer>("反序列化对象失败！", ex);

                return null;
            }
        }
    }
}