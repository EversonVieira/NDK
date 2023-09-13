using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.Core.ExtensionMethods
{
    public static class ResponseExtensions
    {
        public static void HandleException(this NdkResponse response, Exception ex, string? payloadJson = null) 
        {
            response.AddMessage(new NdkMessage
            {
                Code = Guid.NewGuid().ToString(),
                Text = ex.Message,
                Type = NdkMessageType.ERROR,
                AdditionalInfo = $"{JsonSerializer.Serialize(ex)} {(payloadJson is not null ? $"\n PAYLOAD: {payloadJson}":"")}"
            });
        }

        public static void Merge(this NdkResponse ndkResponse, NdkResponse response)
        {
            response.Messages.ForEach(message =>
            {
                ndkResponse.AddMessage(message);
            });
        }
    }
}
