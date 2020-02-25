using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Core
{
    static class Utils
    {
        public static string FrmUrlSafeBase64(string base64)
        {
            base64 = base64.Replace('-', '+').Replace('_', '/').PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');

            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
    }
}
