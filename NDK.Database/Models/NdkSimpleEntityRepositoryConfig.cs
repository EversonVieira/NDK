using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.Models
{
    public class NdkSimpleEntityRepositoryConfig
    {
        private Dictionary<Type, string> _entityTableMap = new Dictionary<Type, string>();

        public NdkSimpleEntityRepositoryConfig(Dictionary<Type, string> entityTableMap)
        {
            _entityTableMap = entityTableMap;
        }

        public string GetEntityTable(Type type) 
        {
            return _entityTableMap[type];
        }
    }
}
