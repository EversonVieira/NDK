using NDK.Auth.Core.Interfaces;
using NDK.Auth.Core.Models;
using NDK.Core.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.Auth.Core.Handlers
{
    public class PublicNdkTokenHandler : IPublicNdkTokenHandler
    {
        public NdkToken RetrieveTokenByString(string token)
        {
            NdkToken result = new NdkToken();

            string[] tokenParts = token.Split('.');

            result.Header = JsonSerializer.Deserialize<NdkTokenHeader>(Encoding.UTF8.GetString(Convert.FromBase64String(handleTokenReplace(tokenParts[0]))));
            result.Payload = JsonSerializer.Deserialize<NdkTokenPayload>(Encoding.UTF8.GetString(Convert.FromBase64String(handleTokenReplace(tokenParts[1]))));
            result.Signature = JsonSerializer.Deserialize<NdkTokenSignature>(Encoding.UTF8.GetString(Convert.FromBase64String(handleTokenReplace(tokenParts[2]))));

            return result;

            string handleTokenReplace(string vlr)
            {
                return vlr.HandleBase64String().Replace("-", "+").Replace("_", @"//");
            }
        }

    }
}
