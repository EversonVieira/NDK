using NDK.Database.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.ExtensionMethods.Internal
{
    internal static class NdkDbConnectionConfigurationExtensions
    {
        public static string GetParamSymbol(this NdkDbConnectionConfiguration configuration)
        {
            if (configuration == null)  throw new InvalidOperationException("NdkDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NdkDbType.ORACLE => ":",
                _ => "@"
            };
        }

        public static string GetInsertSelectResult(this NdkDbConnectionConfiguration configuration)
        {
            if (configuration == null) throw new InvalidOperationException("NdkDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NdkDbType.SQLSERVER => "SELECT SCOPE_IDENTITY()",
                NdkDbType.MYSQL => "SELECT last_insert_id()",
                _ => "SELECT 0"
            };
        }

        public static string GetUpdateSelectResult(this NdkDbConnectionConfiguration configuration)
        {
            if (configuration == null) throw new InvalidOperationException("NdkDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NdkDbType.SQLSERVER => "SELECT SCOPE_IDENTITY()",
                NdkDbType.MYSQL => "SELECT last_insert_id()",
                _ => "SELECT 0"
            };
        }

        public static string GetUpdateOrDeleteResult(this NdkDbConnectionConfiguration configuration)
        {
            if (configuration == null) throw new InvalidOperationException("NdkDbConnectionConfiguration wasn't provided.");

            return configuration.Type switch
            {
                NdkDbType.SQLSERVER => "SELECT SELECT @@ROWCOUNT",
                NdkDbType.MYSQL => "SSELECT ROW_COUNT()",
                _ => "SELECT 0"
            };
        }
    }
}
