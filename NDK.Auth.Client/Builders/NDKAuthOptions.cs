using NDK.Auth.Client.Interfaces;
using NDK.Auth.Core.Models;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Client.Builders
{
    public class NDKAuthOptions<TToken, TUser>
        where TToken : NDKToken
        where TUser : NDKUser
    {
        public INDKTokenClientHandler<TToken, TUser>? _handler { get; set; }

        public NDKAuthOptions<TToken, TUser> AddHandler(INDKTokenClientHandler<TToken> handler)
        {
            _handler = handler;
            return this;
        }

    }

    public class NDKAuthOptions : NDKAuthOptions<NDKToken, NDKUser>
    {

    }
}
