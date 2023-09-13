using NDK.Core.ExtensionMethods;
using NDK.Core.Models;
using NDK.SendMail.Server.Consumer;
using NDK.SendMail.Server.Repository;
using NDK.SendMail.Server.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.SendMail.Server.Business
{
    public class MailBusiness
    {
        private readonly SMTPConsumer _consumer;
        private readonly MailRepository _mailRepository;
        private readonly ResponseLogRepository _responseLogRepository;
        private readonly MailValidator _mailValidator;

        public MailBusiness(SMTPConsumer consumer,
                            MailRepository mailRepository, 
                            ResponseLogRepository responseLogRepository, 
                            MailValidator mailValidator)
        {
            _consumer = consumer;
            _mailRepository = mailRepository;
            _responseLogRepository = responseLogRepository;
            _mailValidator = mailValidator;
        }

        public async Task<NdkResponse> SendPendingEmailsAsync()
        {
            var response = await _mailRepository.GetMailsToSendAsync();

            if (response.HasAnyMessage || response.Result is null) 
            {
                return response;
            }

            foreach (var mail in response.Result) 
            {
                var validateResponse = await _mailValidator.ValidateMailAsync(mail);
                if (validateResponse.HasAnyMessage)
                {
                    response.Merge(validateResponse);
                    continue;
                }

                var result = await _consumer.SendAsync(mail);
                response.Merge(result);

                if (!result.HasAnyMessage) 
                {
                    response.Merge(await _mailRepository.InsertEmailAsync(mail));
                }
            }

            if (response.HasAnyMessage)
            {
                await _responseLogRepository.LogAsync(response);
            }

            return response;
        }
    }
}
