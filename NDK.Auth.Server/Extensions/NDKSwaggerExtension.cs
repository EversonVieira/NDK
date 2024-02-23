using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NDK.Auth.Server.Extensions
{
    public static class NDKSwaggerExtensions
    {
        public static void AddNDKToken(this SwaggerGenOptions options, string tokenName)
        {
            var c = options;

            if (string.IsNullOrEmpty(tokenName))
            {
                throw new ArgumentNullException(nameof(tokenName));
            }


            c.AddSecurityDefinition(tokenName, new OpenApiSecurityScheme
            {
                Description = @$"{tokenName} of the current logged user.",
                Name = tokenName,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = tokenName
                        },
                        Name = tokenName,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }
    }
}
