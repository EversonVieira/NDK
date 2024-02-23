using NDK.Auth.Core.Models;
using NDK.Core.Models;
using NDK.Core.Extensions;
using System.Text.Json;
using System.Text;
using NDK.Auth.Core.Interfaces;

namespace NDK.Auth.Core.Abstract
{
    public abstract class NDKTokenHandler<TToken> : INDKTokenHandler<TToken> where TToken : NDKToken
    {
        public virtual async Task<TToken> RetrieveTokenByStringAsync(string token)
        {
            TToken result = Activator.CreateInstance<TToken>();

            string[] tokenParts = token.Split('.');

            result.Header = JsonSerializer.Deserialize<NDKTokenHeader>(Encoding.UTF8.GetString(Convert.FromBase64String(handleTokenReplace(tokenParts[0]))));
            result.Payload = JsonSerializer.Deserialize<NDKTokenPayload>(Encoding.UTF8.GetString(Convert.FromBase64String(handleTokenReplace(tokenParts[1]))));
            result.Signature = JsonSerializer.Deserialize<NDKTokenSignature>(Encoding.UTF8.GetString(Convert.FromBase64String(handleTokenReplace(tokenParts[2]))));

            await Task.CompletedTask;

            return result;

            string handleTokenReplace(string vlr)
            {
                return vlr.HandleBase64String().Replace("-", "+").Replace("_", @"//");
            }
        }

        public abstract Task<NDKResponse<bool>> ValidateTokenAsync(TToken token);
    }
}