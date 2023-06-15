using NDK.Core.Models;
using System.Data.Common;

namespace NDK.Database.Interfaces
{
    public interface INdkRepository<T>:INdkBaseRepository where T : NdkBaseModel
    {
        public NdkResponse<long> Insert (T entity, DbConnection connection);
        public NdkResponse<long> Update (T entity, DbConnection connection);
        public NdkResponse<long> Delete (T entity, DbConnection connection);
        public NdkListResponse<T> GetByRequest(NdkRequest request, DbConnection connection);
    }
}
