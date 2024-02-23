using NDK.Auth.Core.Models;

namespace NDK.Auth.Core.Interfaces
{
    public interface IPublicNdkTokenHandler
    {
        NdkToken RetrieveTokenByString(string token);
    }
}