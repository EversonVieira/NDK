using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Core.Models
{
    public class NDKToken
    {
        public NDKTokenHeader? Header { get; set; }
        public NDKTokenPayload? Payload { get; set; }
        public NDKTokenSignature? Signature { get; set; }

    }

}
