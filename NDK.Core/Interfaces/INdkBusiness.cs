using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Interfaces
{
    public interface INdkBusiness<T>
    {
        public NdkResponse<long> Insert(T entity);
        public NdkResponse<long> Update(T entity);
        public NdkResponse<long> Delete(T entity);
        public NdkListResponse<T> GetByRequest(NdkRequest request);
    }
}
