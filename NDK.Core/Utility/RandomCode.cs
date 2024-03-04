using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Utility
{
    public static class RandomCode
    {
        public static string GenerateRandomCode(int length)
        {
            StringBuilder sb = new StringBuilder();

            Random rdm = new Random();

            for (int i = 0; i < length;i++)
            {
               sb.Append(rdm.Next(0,9));
            }
            
            return sb.ToString();
        }
    }
}
