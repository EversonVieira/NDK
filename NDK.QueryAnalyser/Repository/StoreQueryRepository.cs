using Dapper;
using NDK.Core.Models;
using NDK.Database.ExtensionMethods;
using NDK.Database.Interfaces;
using NDK.Database.Models;
using NDK.QueryAnalyser.Models;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.QueryAnalyser.Repository
{
    public class StoreQueryRepository
    {
        private INdkDbConnectionFactory _connectionFactory;
        private NdkDbConnectionConfiguration _connectionConfiguration;
        public StoreQueryRepository(INdkDbConnectionFactory dbConnectionFactory, NdkDbConnectionConfiguration connectionConfiguration) 
        { 
            _connectionFactory = dbConnectionFactory;
            _connectionConfiguration = connectionConfiguration;
        }

        public static string INSERT =
            @"INSERT INTO STOREQUERY(QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil) 
              VALUES(@QueryMD5, @TriggeredBy, @FullQuery, @BaseQuery, @RequestJson, @RunnedAt, @RunnedUntil)";

        public static string SELECT =
            @"SELECT Id, QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil FROM STOREQUERY ";


        public async Task Insert(StoreQuery storeQuery)
        {
            using (var conn = _connectionFactory.GetDbConnection())
            {
                conn.Open();
                
                await conn.ExecuteAsync(INSERT, storeQuery);
            }
        }

        public async Task<List<StoreQuery>> SelectByRequest(NdkRequest request)
        {
            List<StoreQuery> result = new List<StoreQuery>();


            using (var conn = _connectionFactory.GetDbConnection())
            {
                conn.Open();

                var data = request.GetRequestData(SELECT, _connectionConfiguration);
                string command = data.query;

                result = (await conn.QueryAsync<StoreQuery>(command,data.parameters)).ToList();
            }

            return result;
        }
    }
}
