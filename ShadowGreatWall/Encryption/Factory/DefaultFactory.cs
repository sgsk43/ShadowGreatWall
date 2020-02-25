using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Encryption.Factory
{
    class DefaultFactory : IEncryptionFactory
    {
        public override void Init()
        {
            Register("none");
        }
    }
}
