using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Interfaces
{
    public interface INDKDataSourceService<TOutput, TInput> 
        where TInput: NDKRequest
        where TOutput : NDKBaseModel
    {
        public Task<NDKListResponse<TOutput>> FetchAsync(TInput request);
    }
}
