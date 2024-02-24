using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.Models
{
    public abstract class NDKSimpleEntityRepositoryConfig
    {
        private Dictionary<Type, string> _entityTableMap = new Dictionary<Type, string>();


        public string GetEntityTable(Type type) 
        {
            return _entityTableMap[type];
        }

        protected void AddMap(Type type, string Table) 
        {
            _entityTableMap.Add(type, Table);
        }

        protected abstract void SetupMap();
    }
}
