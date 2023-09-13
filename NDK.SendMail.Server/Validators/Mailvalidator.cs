using NDK.Core.Models;
using NDK.SendMail.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.SendMail.Server.Validators
{
    public class MailValidator
    {

        public async Task<NdkResponse> ValidateMailAsync(NdkMailMessage message)
        {
            NdkResponse response = new NdkResponse();


            return response;
        }
    }
}
