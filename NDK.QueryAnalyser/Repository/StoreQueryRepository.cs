using Dapper;
using NDK.Core.Models;
using NDK.Database.ExtensionMethods;
using NDK.QueryAnalyser.Handlers;
using NDK.QueryAnalyser.Models;

namespace NDK.QueryAnalyser.Repository
{
    public class StoreQueryRepository
    {
        private SqlAnalyserConnectionHandler _sqlAnalyserConnectionFactory;
        public StoreQueryRepository(SqlAnalyserConnectionHandler sqlAnalyserConnectionFactory) 
        { 
            _sqlAnalyserConnectionFactory = sqlAnalyserConnectionFactory;
        }

        public static string INSERT =
            @"INSERT INTO STOREQUERY(QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil) 
              VALUES(@QueryMD5, @TriggeredBy, @FullQuery, @BaseQuery, @RequestJson, @RunnedAt, @RunnedUntil)";

        public static string SELECT =
            @"SELECT Id, QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil FROM STOREQUERY ";


        public async Task Insert(StoreQuery storeQuery)
        {
            using (var conn = _sqlAnalyserConnectionFactory.GetDbConnection())
            {
                conn.Open();
                
                await conn.ExecuteAsync(INSERT, storeQuery);
            }
        }

        public async Task<List<StoreQuery>> SelectByRequest(NdkRequest request)
        {
            List<StoreQuery> result = new List<StoreQuery>();


            using (var conn = _sqlAnalyserConnectionFactory.GetDbConnection())
            {
                conn.Open();

                var data = request.GetRequestData(SELECT, _sqlAnalyserConnectionFactory._configuration);
                string command = data.query;

                result = (await conn.QueryAsync<StoreQuery>(command,data.parameters)).ToList();
            }

            return result;
        }
    }
}
