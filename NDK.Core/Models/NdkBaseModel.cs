using NDK.Core.Attributes;
using NDK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class NdkBaseModel:IPersistable, IComparable<NdkBaseModel>
    {
        public long Id { get; set; }
        public Guid Uuid { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastUpdatedBy { get; set;}
        public DateTime LastUpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public NdkBaseModel() 
        {
            Uuid = Guid.NewGuid();
        }

        public int CompareTo(NdkBaseModel? other)
        {
            if (other is null) return 1;

            if (ReferenceEquals(this, other)) return 0;

            if (this.Id == other.Id) return 0;

            if (this.Id < other.Id) return -1;

            if (this.Id > other.Id) return 1;


            return 0;
        }
    }
}
