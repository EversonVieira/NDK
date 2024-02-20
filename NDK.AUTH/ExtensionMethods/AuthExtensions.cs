﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NDK.Auth.Handlers;
using NDK.Auth.Interfaces;
using NDK.Auth.MiddleWares;
using NDK.PublicAuth.Handlers;
using NDK.PublicAuth.Interfaces;
using NDK.PublicAuth.Models;

namespace NDK.Auth.ExtensionMethods
{
    public static class AuthExtensions
    {

        public static void UseNdkAuthMiddleWare(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<NdkAuthMiddleWare>();
        }

        public static void AddNdkAuthServices(this IServiceCollection services, NdkTokenConfiguration tokenConfiguration)
        {
            services.AddSingleton(tokenConfiguration);
            services.AddSingleton<IPublicNdkTokenHandler, PublicNdkTokenHandler>();
            services.AddSingleton<INdkTokenHandler, NdkTokenHandler>();
        }
    }
}
