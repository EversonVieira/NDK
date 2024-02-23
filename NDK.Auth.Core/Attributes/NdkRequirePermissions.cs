using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Core.Attributes
{
    internal class NDKRequirePermissions : Attribute
    {
        public string[] Permissions { get; set; }

        public NDKRequirePermissions(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
