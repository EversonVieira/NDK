using FluentValidation;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.ExtensionMethods
{
    public static class FluentValidatorExtensions
    {
        public static NdkResponse ValidateAsNdkResponse<T>(this AbstractValidator<T> validator, T obj)
        {
            NdkResponse response = new NdkResponse();

            var result = validator.Validate(obj);

            if (!result.IsValid)
            {
                result.Errors.ForEach(e =>
                {
                    response.AddMessage(new NdkMessage
                    {
                        Code = e.ErrorCode,
                        Text = e.ErrorMessage,
                        Type = NdkMessageType.VALIDATION
                    });
                });
            }

            return response;
        }
    }
}
