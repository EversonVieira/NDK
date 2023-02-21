using Microsoft.Extensions.Logging;
using NDK.Core.Models;
using NDK.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.Base
{
    public class BaseRepository
    {
        public const string BASE_INSERT_SQL_COLUMNS = "IsActive, IsDeleted, CreatedBy, CreatedAt, LastUpdatedBy, LastUpdatedAt";
        public const string BASE_INSERT_SQL_VALUES = "@IsActive, @IsDeleted, @CreatedBy, @CreatedAt, @LastUpdatedBy, @LastUpdatedAt";
        public const string BASE_SELECT_SQL = "IsActive, IsDeleted, CreatedBy, CreatedAt, LastUpdatedBy, LastUpdatedAt";
        public const string BASE_UPDATE_SQL = "IsActive = @IsActive, IsDeleted = @IsDeleted, CreatedBy = @CreatedBy, CreatedAt = @CreatedAt, LastUpdatedBy = @LastUpdatedBy, LastUpdatedAt = @LastUpdatedAt";

        public void HandleException(Exception exception, NdkResponse response, ILogger logger)
        {
            logger.LogError(exception.Message, exception);
            response.AddMessage(new NdkMessage
            {
                Code = $"EX{RandomCode.GenerateRandomCode(5)}_{DateTime.UtcNow}",
                Text = "An internal error happened, try again and if the problems persists contact the support with the exception code.",
                Type = NdkMessageType.EXCEPTION
            });
        }
    }
}
