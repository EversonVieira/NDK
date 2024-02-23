using NDK.Auth.Core.Models;
using NDK.Core.Models;

namespace NDK.Auth.Core.Interfaces
{
    public interface INDKTokenHandler<TToken> where TToken : NDKToken
    {
        Task<TToken> RetrieveTokenByStringAsync(string token);
        Task<NDKResponse<bool>> ValidateTokenAsync(TToken token);
    }
}