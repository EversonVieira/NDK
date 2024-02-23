using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Core.Attributes
{
    public class NdkRequireRoles
    {
        public string[] Roles { get; set; }

        public NdkRequireRoles(params string[] roles)
        {
            Roles = roles;
        }

    }
}
