using NDK.Auth.Core.Interfaces;
using NDK.Auth.Core.Models;
using NDK.Core.Models;

namespace NDK.Auth.Client.Interfaces
{
    public interface INDKTokenClientHandler<TToken,TUser>:INDKTokenHandler<TToken,TUser> 
        where TToken: NDKToken
        where TUser:NDKUser
    {

    }
}
