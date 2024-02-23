using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NDKRequest
    {
        public NDKPaging? Paging { get; set; }


        private List<NDKFilterGroup> _filtersGroups = new List<NDKFilterGroup>();

        public List<NDKOrderItem>? OrderBy { get; set; }

        public IReadOnlyList<NDKFilterGroup> FiltersGroups
        {
            get
            {
                return _filtersGroups;
            }
        }

        public void ClearFilters()
        {
            _filtersGroups.Clear();
        }

        public void ClearPaging()
        {
            Paging = null;
        }

        public void ClearOrderBy()
        {
            OrderBy = null;
        }

        public void AddFilterGroup(NDKFilterGroup filterGroup)
        {
            filterGroup.Id = (_filtersGroups.Count + 1).ToString();

            _filtersGroups.Add(filterGroup);
        }

        public void AddFilterToGroup(NDKFilter filter, NDKFilterGroup group)
        {
            if (group is null) throw new ArgumentNullException(nameof(group));
            
            filter.Id = $"{group.Id}_{(group?.Filters?.Count ?? 0)+ 1}";

            if (group!.Filters is null)
                group.Filters = new List<NDKFilter>();
            group.Filters.Add(filter);
        }
    }
}
