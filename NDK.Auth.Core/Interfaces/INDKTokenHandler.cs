using NDK.Auth.Core.Models;
using NDK.Core.Models;

namespace NDK.Auth.Core.Interfaces
{
    public interface INDKTokenHandler<TToken, TUser> 
        where TToken : NDKToken
        where TUser : NDKUser
    {
        Task<TToken> RetrieveTokenByStringAsync(string token);
        Task<NDKResponse<bool>> ValidateTokenAsync(TToken token);
        Task<NDKResponse<TUser>> RetrieveUserByTokenAsync(TToken token);
        Task<NDKResponse<bool>> HasPermissionAsync(string permission);
        Task<NDKResponse<bool>> HasRoleAsync(string permission);
    }
}