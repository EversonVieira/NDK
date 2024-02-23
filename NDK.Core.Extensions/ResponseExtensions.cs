using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.Core.Extensions
{
    public static class ResponseExtensions
    {
        public static void HandleException(this NDKResponse response, Exception ex, string? payloadJson = null)
        {
            response.AddMessage(new NDKMessage
            {
                Code = Guid.NewGuid().ToString(),
                Text = ex.Message,
                Type = NDKMessageType.ERROR,
                AdditionalInfo = $"{JsonSerializer.Serialize(ex)} {(payloadJson is not null ? $"\n PAYLOAD: {payloadJson}" : "")}"
            });
        }

        public static void Merge(this NDKResponse ndkResponse, NDKResponse response)
        {
            response.Messages.ForEach(message =>
            {
                ndkResponse.AddMessage(message);
            });
        }
    }
}
