using Microsoft.Extensions.Logging;
using NDK.Core.Models;
using NDK.Core.Utility;
using NDK.Database.Interfaces;
using System;

namespace NDK.Database.Base
{
    public class NdkBaseRepository: INdkBaseRepository
    {
        public const string BASE_INSERT_SQL_COLUMNS = "Uuid, IsActive, IsDeleted, CreatedBy, CreatedAt, LastUpdatedBy, LastUpdatedAt";
        public const string BASE_INSERT_SQL_VALUES = "@IsActive, @IsDeleted, @CreatedBy, @CreatedAt, @LastUpdatedBy, @LastUpdatedAt";
        public const string BASE_SELECT_SQL = "Uuid, IsActive, IsDeleted, CreatedBy, CreatedAt, LastUpdatedBy, LastUpdatedAt";
        public const string BASE_UPDATE_SQL = "Uuid = @Uuid, IsActive = @IsActive, IsDeleted = @IsDeleted, CreatedBy = @CreatedBy, CreatedAt = @CreatedAt, LastUpdatedBy = @LastUpdatedBy, LastUpdatedAt = @LastUpdatedAt";

        public void HandleException(Exception exception, NdkResponse response, ILogger logger)
        {
            logger.LogError(exception.Message, exception);
            response.AddMessage(new NdkMessage
            {
                Code = $"EX{RandomCode.GenerateRandomCode(5)}_{DateTime.UtcNow}",
                Text = "An internal error happened, try again and if the problems persists contact the support with the exception code.",
                Type = NdkMessageType.EXCEPTION,
            });
        }

        protected virtual void ApplyBaseModelValues<User, Model>(User loggedUser, Model model ) where User : NdkUser
                                                                                where Model:NdkBaseModel
        {

            model.CreatedAt = DateTime.UtcNow;
            model.LastUpdatedAt = DateTime.UtcNow;
            model.CreatedBy = $"{loggedUser.FirstName} {loggedUser.LastName}";
            model.LastUpdatedBy = $"{loggedUser.FirstName} {loggedUser.LastName}";
        }

        

    }


}
