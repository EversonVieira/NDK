using NDK.PublicAuth.Models;

namespace NDK.Auth.Interfaces
{
    public interface INdkAuthUserProvider<AuthUser, Token> where Token:NdkToken
    {
        AuthUser GetLoggedUserByToken();
    }
}