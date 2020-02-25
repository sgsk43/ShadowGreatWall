using Org.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadowGreatWall.Core.Serializer
{
    internal abstract class ISerializer
    {
        public abstract string Serialize(object Data);
        public T Desrialize<T>(string Data)
        {
            try
            {
                object obj = Desrialize(typeof(T), Data);

                return (T)obj;
            }
            catch (Exception ex)
            {
                AppLogProxy.AppLog.WriteLog<CustomXmlSerializer>("反序列化对象失败！", ex);

                return default(T);
            }
        }

        public abstract object Desrialize(Type Type, string Data);
    }
}