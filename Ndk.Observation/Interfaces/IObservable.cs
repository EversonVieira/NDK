using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndk.Observation.Interfaces
{
    public interface IObservable
    {
        public void Update();
    }

    public interface ISubject<T>
    {

    }
}
