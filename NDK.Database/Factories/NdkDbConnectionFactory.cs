using MySql.Data.MySqlClient;
using NDK.Database.Interfaces;
using NDK.Database.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace NDK.Database.Factories
{
    public class NdkDbConnectionFactory : INdkDbConnectionFactory
    {
        public NdkDbConnectionConfiguration _configuration { get; set; };

        public NdkDbConnectionFactory(NdkDbConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetDbConnection()
        {
            return _configuration.DBType switch
            {
                NdkDbType.SQLSERVER => new SqlConnection(_configuration.ConnectionString),
                NdkDbType.MYSQL => new MySqlConnection(_configuration.ConnectionString),
                // TO DO, IMPLEMENT ORACLE, POSTGREE AND MARIADB
                _ => throw new Exception("Not implemented")
            };
        }
    }
}
