using MongoDB.Driver;
using NDK.Core.ExtensionMethods;
using NDK.Core.Models;
using NDK.SendMail.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.SendMail.Server.Repository
{
    public class ResponseLogRepository
    {
        private MongoClient _client;

        public ResponseLogRepository(MongoClient client)
        {
            _client = client;
        }

        public async Task<NdkResponse> LogAsync<T>(T response) where T : NdkResponse
        {
            NdkResponse result = new NdkResponse();

            try
            {
                var collection = _client.GetDatabase("ResponseLogs").GetCollection<T>("MailMessages");

                await collection.InsertOneAsync(response);
            }
            catch (Exception ex)
            {
                response.HandleException(ex, JsonSerializer.Serialize(response));
            }

            return response;
        }
    }
}
