using NDK.Auth.Client.Interfaces;
using NDK.Auth.Core.Models;
using NDK.Core.Models;

namespace NDK.Auth.Client.Builders
{
    public class NDKAuthOptions<TToken, TUser>
        where TToken : NDKToken
        where TUser : NDKUser
    {
        public INDKTokenClientHandler<TToken, TUser>? _handler { get; set; }

        public NDKAuthOptions<TToken, TUser> AddHandler(INDKTokenClientHandler<TToken, TUser> handler)
        {
            _handler = handler;
            return this;
        }

    }

    public class NDKAuthOptions : NDKAuthOptions<NDKToken, NDKUser>
    {

    }
}
