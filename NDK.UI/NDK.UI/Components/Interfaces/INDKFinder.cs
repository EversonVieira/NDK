using NDK.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Interfaces
{
    public interface INDKFinder<TInput,TOutput> 
        where TInput:NDKFinderInput
        where TOutput:NDKFinderOutput
    {
        
        public Task<List<TOutput>> FindAsync(TInput input);
    }
}
