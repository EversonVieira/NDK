using MySql.Data.MySqlClient;
using NDK.Database.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace NDK.Database.Handlers
{
    public class NdkDbConnectionHandler
    {
        public NdkDbConnectionConfiguration _configuration { get; set; }

        public NdkDbConnectionHandler(NdkDbConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetDbConnection()
        {
            DbConnection conn = _configuration.DBType switch
            {
                NdkDbType.SQLSERVER => new SqlConnection(_configuration.ConnectionString),
                NdkDbType.MYSQL => new MySqlConnection(_configuration.ConnectionString),
                // TO DO, IMPLEMENT ORACLE, POSTGREE AND MARIADB
                _ => throw new Exception("Not implemented")
            };

            conn.Open();

            return conn;
        }
    }

}
