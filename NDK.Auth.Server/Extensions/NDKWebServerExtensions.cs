using Microsoft.Extensions.DependencyInjection;
using NDK.Auth.Server.Builders;

namespace NDK.Auth.Server.Extensions
{
    public static class NDKWebServerExtensions
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
