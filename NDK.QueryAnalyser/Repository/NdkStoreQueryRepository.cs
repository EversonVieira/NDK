using Dapper;
using NDK.Core.Models;
using NDK.Database.ExtensionMethods;
using NDK.QueryAnalyser.Core.Handlers;
using NDK.QueryAnalyser.Core.Models;

namespace NDK.QueryAnalyser.Core.Repository
{
    public class NDKStoreQueryRepository
    {
        private SqlAnalyserConnectionHandler _sqlAnalyserConnectionFactory;
        public NDKStoreQueryRepository(SqlAnalyserConnectionHandler sqlAnalyserConnectionFactory)
        {
            _sqlAnalyserConnectionFactory = sqlAnalyserConnectionFactory;
        }

        public static string INSERT =
            @"INSERT INTO STOREQUERY(QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil) 
              VALUES(@QueryMD5, @TriggeredBy, @FullQuery, @BaseQuery, @RequestJson, @RunnedAt, @RunnedUntil)";

        public static string SELECT =
            @"SELECT Id, QueryMD5, TriggeredBy, FullQuery, BaseQuery, RequestJson, RunnedAt, RunnedUntil FROM STOREQUERY ";


        public async Task Insert(NDKStoreQuery storeQuery)
        {
            using (var conn = _sqlAnalyserConnectionFactory.GetDbConnection())
            {
                conn.Open();

                await conn.ExecuteAsync(INSERT, storeQuery);
            }
        }

        public async Task<List<NDKStoreQuery>> SelectByRequest(NDKRequest request)
        {
            List<NDKStoreQuery> result = new List<NDKStoreQuery>();


            using (var conn = _sqlAnalyserConnectionFactory.GetDbConnection())
            {
                conn.Open();

                var data = request.GetRequestData(SELECT, _sqlAnalyserConnectionFactory._configuration);
                string command = data.query;

                result = (await conn.QueryAsync<NDKStoreQuery>(command, data.parameters)).ToList();
            }

            return result;
        }
    }
}
