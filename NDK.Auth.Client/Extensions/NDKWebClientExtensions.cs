using Microsoft.Extensions.DependencyInjection;
using NDK.Auth.Client.Builders;

namespace NDK.Auth.Client.Extensions
{
    public static class NDKWebClientExtensions
    {
        public static void AddNDKAuth(this IServiceCollection services, Action<NDKAuthOptions> options)
        {
            NDKAuthOptions obj = new NDKAuthOptions();

            options(obj);

            if (obj._handler is not null)
            {
                services.AddSingleton(obj._handler);
            }
        }
    }
}
