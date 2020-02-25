using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowGreatWall.Encryption.Factory;
using System.Reflection;

namespace ShadowGreatWall.Encryption
{
    static class EncryptionCenter
    {
        #region [变量]
        private static Dictionary<string, IEncryptionFactory> encryptors = new Dictionary<string, IEncryptionFactory>();
        #endregion

        #region [接口]
        public static void Init()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(IEncryptionFactory)) && type != typeof(IEncryptionFactory))
                {
                    ((IEncryptionFactory)Activator.CreateInstance(type)).Init();
                }
            }
        }

        public static void Register(string key, IEncryptionFactory factory)
        {
            key = key.ToLower();

            if (encryptors.ContainsKey(key))
            {
                throw new Exception("加密标志已注册 => " + key);
            }

            encryptors.Add(key, factory);
        }

        public static void QueryAllKeys(Action<string> action)
        {
            foreach (string key in encryptors.Keys)
            {
                action.Invoke(key);
            }
        }

        public static bool IsSupport(string key)
        {
            return encryptors.ContainsKey(key.ToLower());
        }
        #endregion
    }
}
