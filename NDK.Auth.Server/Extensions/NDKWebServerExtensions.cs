using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NDK.Auth.Core.Models;
using NDK.Auth.Server.Builders;
using NDK.Auth.Server.Interfaces;
using NDK.Core.Models;

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
