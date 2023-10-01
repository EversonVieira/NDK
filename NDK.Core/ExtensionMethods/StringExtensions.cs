using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string HandleBase64String(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return obj;
            }

            while (obj.Length % 4 == 0) 
            {
                obj = $"{obj}=";
            }

            return obj;
        }
    }
}
