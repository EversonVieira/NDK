using NDK.Core.Models;
using System.Data.Common;

namespace NDK.Database.Interfaces
{
    public interface INDKRepository<T>:INDKBaseRepository where T : NDKBaseModel
    {
        public NDKResponse<long> Insert (T entity, DbConnection connection);
        public NDKResponse<long> Update (T entity, DbConnection connection);
        public NDKResponse<long> Delete (T entity, DbConnection connection);
        public NDKListResponse<T> GetByRequest(NDKRequest request, DbConnection connection);
    }
}
