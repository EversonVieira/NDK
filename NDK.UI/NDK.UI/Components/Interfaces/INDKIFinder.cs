using NDK.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Interfaces
{
    public interface INDKIFinder<TInput,TOutput> 
        where TInput:NDKFinderInput
        where TOutput:NDKFinderOutput
    {
        
        public List<TOutput> FindByInput(TInput input);


    }
}
