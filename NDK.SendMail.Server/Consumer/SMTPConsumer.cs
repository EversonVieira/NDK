using NDK.Core.ExtensionMethods;
using NDK.Core.Models;
using NDK.SendMail.Shared;
using System.Net.Mail;
using System.Text.Json;

namespace NDK.SendMail.Server.Consumer
{
    public class SMTPConsumer
    {
        SmtpClient _smtpClient { get; set; }

        public SMTPConsumer(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task<NdkResponse> SendAsync(NdkMailMessage message)
        {
            var result = new NdkResponse();
            try
            {
                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception e)
            {
                result.HandleException(e, JsonSerializer.Serialize(message));
            }

            return result;
        }
    }
}
