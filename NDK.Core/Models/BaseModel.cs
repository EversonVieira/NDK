using NDK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Models
{
    public class BaseModel<T>:IPersistable
    {
        public T? Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastUpdatedBy { get; set;}
        public DateTime LastUpdatedAt { get; set; }
    }
}
