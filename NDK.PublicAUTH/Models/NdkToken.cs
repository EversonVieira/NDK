using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.PublicAuth.Models
{
    public class NdkToken
    {
        public NdkTokenHeader? Header { get; set; }
        public NdkTokenPayload? Payload { get; set; }
        public NdkTokenSignature? Signature { get; set; }

    }

}
