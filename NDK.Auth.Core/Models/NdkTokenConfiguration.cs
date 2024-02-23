using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Core.Models
{
    public class NdkTokenConfiguration
    {
        public string PrivateKey { get; set; } = string.Empty;
        public string? Signer { get; set; }
        public string? TokenType { get; set; }
        public int ExpirationInMinutes { get; set; }

        public string Gen1 { get; set; } = string.Empty;
        public string Gen2 { get; set; } = string.Empty;
    }
}
