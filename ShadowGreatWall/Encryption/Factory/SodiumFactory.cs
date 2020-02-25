using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Encryption.Factory
{
    class SodiumFactory : IEncryptionFactory
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
            { "salsa20", new EncryptorInfo() { KeySize = 32, IVSize = 8 } },
            { "chacha20", new EncryptorInfo() { KeySize = 32, IVSize = 8 } },
            { "xsalsa20", new EncryptorInfo() { KeySize = 32, IVSize = 24 } },
            { "xchacha20", new EncryptorInfo() { KeySize = 32, IVSize = 24 } },
            { "chacha20-ietf", new EncryptorInfo() { KeySize = 32, IVSize = 12 } },
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
