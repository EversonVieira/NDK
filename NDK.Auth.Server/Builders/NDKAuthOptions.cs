using NDK.Auth.Core.Interfaces;
using NDK.Auth.Core.Models;
using NDK.Auth.Server.Interfaces;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Server.Builders
{
    public class NDKAuthOptions<TToken,TUser> 
        where TToken : NDKToken
        where TUser : NDKUser
    {
        public INDKTokenServerHandler<TToken, TUser>? _handler { get; set; }

        public NDKAuthOptions<TToken,TUser> AddHandler(INDKTokenServerHandler<TToken, TUser> handler) 
        {
            _handler = handler;
            return this;
        }
        
    }

    public class NDKAuthOptions:NDKAuthOptions<NDKToken, NDKUser>
    {

    }
}
