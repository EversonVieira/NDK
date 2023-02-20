using NDK.PublicAuth.Models;

namespace NDK.PublicAuth.Interfaces
{
    public interface IPublicNdkTokenHandler
    {
        NdkToken RetrieveTokenByString(string token);
    }
}