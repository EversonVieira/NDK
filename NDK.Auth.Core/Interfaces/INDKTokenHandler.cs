using NDK.Auth.Core.Models;
using NDK.Core.Models;

namespace NDK.Auth.Core.Interfaces
{
    public interface INDKTokenHandler<T> where T : NDKToken
    {
        T RetrieveTokenByString(string token);
    }
}