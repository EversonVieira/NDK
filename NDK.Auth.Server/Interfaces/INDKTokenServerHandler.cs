using NDK.Auth.Core.Interfaces;
using NDK.Auth.Core.Models;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Server.Interfaces
{
    public interface INDKTokenServerHandler<TToken,TUser>:INDKTokenHandler<TToken> 
        where TToken : NDKToken
        where TUser :NDKUser
    {
        Task<NDKResponse<TToken>> CreateTokenAsync(TUser input);
        Task<NDKResponse<TUser>> RetrieveUserByToken(TToken token);
    }
}
