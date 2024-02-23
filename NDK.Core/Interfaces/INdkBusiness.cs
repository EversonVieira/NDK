using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Interfaces
{
    public interface INDKBusiness<T>
    {
        public NDKResponse<long> Insert(T entity);
        public NDKResponse<long> Update(T entity);
        public NDKResponse<long> Delete(T entity);
        public NDKListResponse<T> GetByRequest(NDKRequest request);
    }
}
