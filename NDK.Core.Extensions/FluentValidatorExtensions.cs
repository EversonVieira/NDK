using FluentValidation;
using NDK.Core.Models;

namespace NDK.Core.Extensions
{
    public static class FluentValidatorExtensions
    {
        public static NDKResponse ValidateAsNDKResponse<T>(this AbstractValidator<T> validator, T obj)
        {
            NDKResponse response = new NDKResponse();

            var result = validator.Validate(obj);

            if (!result.IsValid)
            {
                result.Errors.ForEach(e =>
                {
                    response.AddMessage(new NDKMessage
                    {
                        Code = e.ErrorCode,
                        Text = e.ErrorMessage,
                        Type = NDKMessageType.VALIDATION
                    });
                });
            }

            return response;
        }
    }
}
