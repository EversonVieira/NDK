using MySql.Data.MySqlClient;
using Nedesk.Database.Interfaces;
using Nedesk.Database.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace Nedesk.Database.Factories
{
    public class NdkDbConnectionFactory : INdkDbConnectionFactory
    {
        private NdkDbConnectionConfiguration _configuration;

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
