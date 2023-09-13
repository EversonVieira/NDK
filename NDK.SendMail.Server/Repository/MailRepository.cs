using MongoDB.Driver;
using NDK.Core.ExtensionMethods;
using NDK.Core.Models;
using NDK.SendMail.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.SendMail.Server.Repository
{
    public class MailRepository
    {
        private MongoClient _client;

        public MailRepository(MongoClient client)
        {
            _client = client;
        }

        public async Task<NdkResponse> InsertEmailAsync(NdkMailMessage message)
        {
            NdkResponse response = new NdkResponse();

            try
            {
                var collection =_client.GetDatabase("SendMail").GetCollection<NdkMailMessage>("MailMessages");

                await collection.InsertOneAsync(message);
            }
            catch (Exception ex)
            {
                response.HandleException(ex, JsonSerializer.Serialize(message));
            }

            return response;
        }

        public async Task<NdkListResponse<NdkMailMessage>> GetMailsToSendAsync()
        {
            NdkListResponse<NdkMailMessage> response = new NdkListResponse<NdkMailMessage>();

            try
            {
                var collection = _client.GetDatabase("Ndk.SendMail").GetCollection<NdkMailMessage>("MailMessages");

                response.Result = (await collection.FindAsync(Builders<NdkMailMessage>.Filter.Eq(x => x.IsSent, false))).ToList();
            }
            catch (Exception ex)
            {
                response.HandleException(ex);

            }

            return response;
        }

    }
}
