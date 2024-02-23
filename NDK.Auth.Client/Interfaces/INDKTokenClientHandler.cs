using NDK.Auth.Core.Interfaces;
using NDK.Auth.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Client.Interfaces
{
    public interface INDKTokenClientHandler<TTOken>:INDKTokenHandler<TTOken> where TTOken: NDKToken
    {

    }
}
