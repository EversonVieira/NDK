using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NDKRef<T>
    {
        private T? _value = default;
        
        public T? Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public NDKRef(T value)
        {
            Value = value;
        }

        public NDKRef() { }

        public static implicit operator T?(NDKRef<T> value)
        {
            return value.Value;
        }

        public static implicit operator NDKRef<T?>(T? value)
        {
            return new NDKRef<T?>(value);
        }
    }
   
}
