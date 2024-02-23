using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Server.Models
{
    public class NdkTokenConfiguration
    {
        public string? PrivateKey { get; set; }
        public string? Signer { get; set; }
        public string? TokenType { get; set; }
        public int? ExpirationInMinutes { get; set; }
        public List<string>? GenStrings { get; set; }
    }
}
