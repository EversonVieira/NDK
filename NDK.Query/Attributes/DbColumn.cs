using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Query.Attributes
{
    public class DbColumn:System.Attribute
    {
        public string Name { get; private set; }

        public DbColumn(string name)
        {
            Name = name;
        }
    }
}
