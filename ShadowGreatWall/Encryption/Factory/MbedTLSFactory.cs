using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Encryption.Factory
{
    class MbedTLSFactory : IEncryptionFactory
    {
        #region [类]
        private class EncryptorInfo
        {
            public int KeySize { get; set; }
            public int IVSize { get; set; }
        }
        #endregion

        #region [变量]
        private Dictionary<string, EncryptorInfo> encryptors = new Dictionary<string, EncryptorInfo> {
            { "aes-128-ctr", new EncryptorInfo() { KeySize = 16, IVSize = 16 } },
            { "aes-192-ctr", new EncryptorInfo() { KeySize = 24, IVSize = 16 } },
            { "aes-256-ctr", new EncryptorInfo() { KeySize = 32, IVSize = 16 } },
            { "aes-128-cfb", new EncryptorInfo() { KeySize = 16, IVSize = 16 } },
            { "aes-192-cfb", new EncryptorInfo() { KeySize = 24, IVSize = 16 } },
            { "aes-256-cfb", new EncryptorInfo() { KeySize = 32, IVSize = 16 } },
            { "bf-cfb", new EncryptorInfo() { KeySize = 16, IVSize = 8 } },
            { "camellia-128-cfb", new EncryptorInfo() { KeySize = 16, IVSize = 16 } },
            { "camellia-192-cfb", new EncryptorInfo() { KeySize = 24, IVSize = 16 } },
            { "camellia-256-cfb", new EncryptorInfo() { KeySize = 32, IVSize = 16 } },
            { "rc4", new EncryptorInfo() { KeySize = 16, IVSize = 0 } },
            { "rc4-md5", new EncryptorInfo() { KeySize = 16, IVSize = 16 } },
            { "rc4-md5-6", new EncryptorInfo() { KeySize = 16, IVSize = 6 } },
        };
        #endregion

        #region [接口]
        public override void Init()
        {
            foreach (string key in encryptors.Keys)
            {
                Register(key);
            }
        }
        #endregion
    }
}
