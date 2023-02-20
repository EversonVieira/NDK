using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NDK.PublicAuth.Handlers;
using NDK.PublicAuth.Interfaces;

namespace NDK.PublicAuth.ExtensionMethods
{
    public static class AuthExtensions
    {

        public static void UseNdkPublicAuthMiddleWare(this IApplicationBuilder builder)
        {
            // TO DO: IMPLEMENT THE FRONT_END AUTH FRAMEWORK
            //builder.UseMiddleware<NdkAuthMiddleWare>();
        }

        public static void AddNdkPublicAuthServices(this IServiceCollection services)
        {
            services.AddSingleton<IPublicNdkTokenHandler, PublicNdkTokenHandler>();
        }
    }
}
