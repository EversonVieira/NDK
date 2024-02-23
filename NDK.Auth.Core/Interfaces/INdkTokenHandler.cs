using NDK.Auth.Core.Models;

namespace NDK.Auth.Core.Interfaces
{
    public interface INdkTokenHandler<T> where T : NdkToken
    {
        T RetrieveTokenByString(string token);
    }
}