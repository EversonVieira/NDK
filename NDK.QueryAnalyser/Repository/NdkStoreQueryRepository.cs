using Dapper;
using NDK.Core.Models;
using NDK.Database.ExtensionMethods;
using NDK.QueryAnalyser.Core.Handlers;
using NDK.QueryAnalyser.Core.Models;

namespace NDK.QueryAnalyser.Core.Repository
{
    public class NdkStoreQueryRepository
    {
        private SqlAnalyserConnectionHandler _sqlAnalyserConnectionFactory;
        public NdkStoreQueryRepository(SqlAnalyserConnectionHandler sqlAnalyserConnectionFactory)
        {
            _sqlAnalyserConnectionFactory = sqlAnalyserConnectionFactory;
        }

        public static string INSERT =
            @"INSERT INTO STOREQUERY(QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil) 
              VALUES(@QueryMD5, @TriggeredBy, @FullQuery, @BaseQuery, @RequestJson, @RunnedAt, @RunnedUntil)";

        public static string SELECT =
            @"SELECT Id, QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil FROM STOREQUERY ";


        public async Task Insert(NdkStoreQuery storeQuery)
        {
            using (var conn = _sqlAnalyserConnectionFactory.GetDbConnection())
            {
                conn.Open();

                await conn.ExecuteAsync(INSERT, storeQuery);
            }
        }

        public async Task<List<NdkStoreQuery>> SelectByRequest(NdkRequest request)
        {
            List<NdkStoreQuery> result = new List<NdkStoreQuery>();


            using (var conn = _sqlAnalyserConnectionFactory.GetDbConnection())
            {
                conn.Open();

                var data = request.GetRequestData(SELECT, _sqlAnalyserConnectionFactory._configuration);
                string command = data.query;

                result = (await conn.QueryAsync<NdkStoreQuery>(command, data.parameters)).ToList();
            }

            return result;
        }
    }
}
