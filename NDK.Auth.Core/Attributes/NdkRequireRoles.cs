using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Core.Attributes
{
    public class NDKRequireRoles
    {
        public string[] Roles { get; set; }

        public NDKRequireRoles(params string[] roles)
        {
            Roles = roles;
        }

    }
}
