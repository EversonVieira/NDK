using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NDKRef<T>
    {
        public T? Value { get; set; }

        public NDKRef(T value)
        {
            Value = value;
        }

        public NDKRef() { }
    }
}
