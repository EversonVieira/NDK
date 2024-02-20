using NDK.Auth.Attributes;
using NDK.Auth.Interfaces;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.Auth.MiddleWares
{
    public class NdkAuthMiddleWare
    {
        private const string TokenAlias = "NdkToken";
        private readonly RequestDelegate _next;
        private readonly INdkTokenHandler _tokenHandler;

        public NdkAuthMiddleWare(RequestDelegate next, INdkTokenHandler ndkTokenHandler)
        {
            _next = next;
            _tokenHandler = ndkTokenHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            NdkAllowAnonymous? attr = endpoint?.Metadata.GetMetadata<NdkAllowAnonymous>();
            NdkRequirePermissions? requirePermissions = endpoint?.Metadata.GetMetadata<NdkRequirePermissions>();
            NdkRequireRoles? ndkRequireRoles = endpoint?.Metadata.GetMetadata<NdkRequireRoles>();

            bool isRequired = attr is null;

            if (isRequired)
            {
                string token = context.Request.Headers[TokenAlias];
                if (string.IsNullOrWhiteSpace(token))
                {
                    await ReturnErrorResponse(context);
                    return;
                }

                NdkResponse<bool> validateResponse = _tokenHandler.ValidateToken(token);

                if (validateResponse.Result && !validateResponse.HasAnyMessage)
                {
                    var originalPayload = _tokenHandler.RetrieveTokenByString(token).Payload;
                    if (originalPayload is null)
                    {
                        throw new InvalidOperationException("Payload can't be null");
                    }

                    var localToken = _tokenHandler.CreateToken(originalPayload);

                    context.Request.Headers[TokenAlias] = _tokenHandler.RetrieveTokenAsString(localToken);

                    await _next(context);
                }
                else
                {

                    await ReturnErrorResponse(context);
                    return;
                }

            }

            await _next.Invoke(context);
        }

        private async Task ReturnErrorResponse(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
