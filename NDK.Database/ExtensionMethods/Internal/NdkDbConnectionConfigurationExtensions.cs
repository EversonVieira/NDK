using NDK.Database.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.ExtensionMethods.Internal
{
    internal static class NDKDbConnectionConfigurationExtensions
    {
        public static string GetParamSymbol(this NDKDbConnectionConfiguration configuration)
        {
            if (configuration == null)  throw new InvalidOperationException("NDKDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NDKDbType.ORACLE => ":",
                _ => "@"
            };
        }

        public static string GetInsertSelectResult(this NDKDbConnectionConfiguration configuration)
        {
            if (configuration == null) throw new InvalidOperationException("NDKDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NDKDbType.SQLSERVER => "SELECT SCOPE_IDENTITY()",
                NDKDbType.MYSQL => "SELECT last_insert_id()",
                _ => "SELECT 0"
            };
        }

        public static string GetUpdateSelectResult(this NDKDbConnectionConfiguration configuration)
        {
            if (configuration == null) throw new InvalidOperationException("NDKDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NDKDbType.SQLSERVER => "SELECT SCOPE_IDENTITY()",
                NDKDbType.MYSQL => "SELECT last_insert_id()",
                _ => "SELECT 0"
            };
        }

        public static string GetUpdateOrDeleteResult(this NDKDbConnectionConfiguration configuration)
        {
            if (configuration == null) throw new InvalidOperationException("NDKDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NDKDbType.SQLSERVER => "SELECT SELECT @@ROWCOUNT",
                NDKDbType.MYSQL => "SSELECT ROW_COUNT()",
                _ => "SELECT 0"
            };
        }
    }
}
