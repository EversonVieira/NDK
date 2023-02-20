using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NdkRequest
    {
        public NdkPaging? Paging { get; set; }

        private List<NdkFilterGroup> _filtersGroups = new List<NdkFilterGroup>();
        public IReadOnlyList<NdkFilterGroup> FiltersGroup
        {
            get
            {
                return _filtersGroups;
            }
        }

        public void AddFilterGroup(NdkFilterGroup filterGroup)
        {
            filterGroup.Id = (_filtersGroups.Count + 1).ToString();

            _filtersGroups.Add(filterGroup);
        }

        public void AddFilterToGroup(NdkFilter filter, NdkFilterGroup group)
        {
            filter.Id = $"{group.Id}_{group.Filters.Count + 1}";
            group.Filters.Add(filter);
        }

    }
}
