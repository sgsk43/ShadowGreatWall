using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ShadowGreatWall.Core.Serializer;

namespace ShadowGreatWall.Core
{
    internal static class SerializerMgr
    {
        private static ISerializer m_oXmlSer = new CustomXmlSerializer();
        private static ISerializer m_oJsonSer = new CustomJsonSerializer();

        public static ISerializer Xml
        {
            get
            {
                return m_oXmlSer;
            }
        }

        public static ISerializer Json
        {
            get
            {
                return m_oJsonSer;
            }
        }
    }
}