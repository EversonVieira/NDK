using MySql.Data.MySqlClient;
using NDK.Database.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace NDK.Database.Handlers
{
    public class NDKDbConnectionHandler
    {
        public NDKDbConnectionConfiguration _configuration { get; set; }

        public NDKDbConnectionHandler(NDKDbConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetDbConnection()
        {
            DbConnection conn = _configuration.Type switch
            {
                NDKDbType.SQLSERVER => new SqlConnection(_configuration.ConnectionString),
                NDKDbType.MYSQL => new MySqlConnection(_configuration.ConnectionString),
                // TO DO, IMPLEMENT ORACLE, POSTGREE AND MARIADB
                _ => throw new Exception("Not implemented")
            };

            conn.Open();

            return conn;
        }
    }

}
