using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NDKRequest
    {
        public NDKPagerModel? Pager { get; set; }
        public NDKFilterStructure? FilterStructure { get; set; }
        public NDKSortBy? SortBy { get; set; }

    }
}
