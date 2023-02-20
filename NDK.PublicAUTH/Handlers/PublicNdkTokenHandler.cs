using NDK.PublicAuth.Interfaces;
using NDK.PublicAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.PublicAuth.Handlers
{
    public class PublicNdkTokenHandler : IPublicNdkTokenHandler
    {
        public NdkToken RetrieveTokenByString(string token)
        {
            NdkToken result = new NdkToken();

            string[] tokenParts = token.Split('.');

            result.Header = JsonSerializer.Deserialize<NdkTokenHeader>(Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[0].Replace("-", "+").Replace("_", @"//"))));
            result.Payload = JsonSerializer.Deserialize<NdkTokenPayload>(Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[1].Replace("-", "+").Replace("_", @"//"))));
            result.Signature = JsonSerializer.Deserialize<NdkTokenSignature>(Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[2].Replace("-", "+").Replace("_", @"//"))));

            return result;
        }

    }
}
