using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Query.Attributes
{
    
    public class DbTable:System.Attribute
    {
        public string Table { get; private set; }

        public DbTable(string tableName)
        {
            Table = tableName;
        }
       
    }
}
