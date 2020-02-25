using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Encryption.Factory
{
    abstract class IEncryptionFactory
    {
        public abstract void Init();

        protected void Register(string key)
        {
            EncryptionCenter.Register(key, this);
        }
    }
}
