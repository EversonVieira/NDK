using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Interfaces
{
    public interface IFinder<TInput, TOutput>
    {
        public TOutput Find(TInput input);
    }
}
