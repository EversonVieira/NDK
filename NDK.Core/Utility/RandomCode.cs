using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Utility
{
    public static class RandomCode
    {
        public static string GenerateRandomCode(int length, int max = 255)
        {
            StringBuilder sb = new StringBuilder();

            if (max > 255)
            {
                throw new ArgumentException("Max should not be greater than 255");
            }

            Random rdm = new Random();

            for (int i = 0; i < length;i++)
            {
               sb.Append(rdm.Next(0,max));
            }
            
            return sb.ToString();
        }
    }
}
