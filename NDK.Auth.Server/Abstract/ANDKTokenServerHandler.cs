using NDK.Auth.Core.Abstract;
using NDK.Auth.Core.Models;
using NDK.Auth.Server.Interfaces;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Auth.Server.Abstract
{
    public class NDKTokenServerHandler<T> : ANDKTokenHandler<T>, INDKTokenServerHandler<T>  where T : NDKToken
    {
      
    }
}
