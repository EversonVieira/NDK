using NDK.Core.Models;
using NDK.PublicAuth.Models;

namespace NDK.Auth.Interfaces
{
    public interface INdkTokenHandler
    {
        NdkToken CreateToken(NdkTokenPayload payload);
        string RetrieveTokenAsString(NdkToken token);
        NdkToken RetrieveTokenByString(string token);
        NdkResponse<bool> ValidateToken(string token);
    }
}