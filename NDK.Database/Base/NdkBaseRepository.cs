using Microsoft.Extensions.Logging;
using NDK.Core.Models;
using NDK.Core.Utility;
using NDK.Database.Handlers;
using NDK.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.Base
{
    public abstract class NDKBaseRepository<TModel, TUser>
        where TModel : NDKBaseModel
        where TUser : NDKUser
    {
        public const string BASE_INSERT_SQL_COLUMNS = "Uuid, IsActive, IsDeleted, CreatedBy, CreatedAt, LastUpdatedBy, LastUpdatedAt";
        public const string BASE_INSERT_SQL_VALUES = "@IsActive, @IsDeleted, @CreatedBy, @CreatedAt, @LastUpdatedBy, @LastUpdatedAt";
        public const string BASE_SELECT_SQL = "Uuid, IsActive, IsDeleted, CreatedBy, CreatedAt, LastUpdatedBy, LastUpdatedAt";
        public const string BASE_UPDATE_SQL = "Uuid = @Uuid, IsActive = @IsActive, IsDeleted = @IsDeleted, CreatedBy = @CreatedBy, CreatedAt = @CreatedAt, LastUpdatedBy = @LastUpdatedBy, LastUpdatedAt = @LastUpdatedAt";

        private readonly NDKDbConnectionHandler _connectionHandler;
        private readonly NDKDbCommandConfiguration _commandConfiguration;
        private readonly ILogger _logger;

        protected NDKBaseRepository(NDKDbConnectionHandler connectionHandler,
                            NDKDbCommandConfiguration commandConfiguration,
                            ILogger logger)
        {
            _connectionHandler = connectionHandler;
            _commandConfiguration = commandConfiguration;
            _logger = logger;
        }

        public virtual void HandleException(Exception exception, NDKResponse response, ILogger logger)
        {
            logger.LogError(exception.Message, exception);
            response.AddMessage(new NDKMessage
            {
                Code = $"EX{RandomCode.GenerateRandomCode(5)}_{DateTime.UtcNow}",
                Text = "An internal error happened, try again and if the problems persists contact the support with the exception code.",
                Type = NDKMessageType.EXCEPTION,
            });
        }

        public virtual void ApplyBaseModelValues(TUser loggedUser, TModel model)
        {
            model.CreatedAt = DateTime.UtcNow;
            model.LastUpdatedAt = DateTime.UtcNow;
            model.CreatedBy = $"{loggedUser.FirstName} {loggedUser.LastName}";
            model.LastUpdatedBy = $"{loggedUser.FirstName} {loggedUser.LastName}";
        }

        protected virtual async Task HandleCommandAsync<TInput>(TInput input, Action<TInput> action)
        {
            await Task.Run(() => action(input));
        }

        protected void HandleCommand<TInput>(TInput input, Action<TInput> action)
        {
            HandleCommandAsync(input, action).RunSynchronously();
        }

        protected virtual async Task<TOutput> HandleCommandAsync<TInput,TOutput>(TInput input, Func<TInput,TOutput> func)
        {
           return await Task.Run(() => func(input));
        }

        protected TOutput HandleCommand<TInput, TOutput>(TInput input, Func<TInput, TOutput> func)
        {
            var task = HandleCommandAsync(input, func);

            task.RunSynchronously();

            return task.Result;
        }
    }
}
