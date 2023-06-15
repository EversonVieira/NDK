using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDK.Core.Interfaces;

namespace NDK.Core.Models
{
    public class NdkUser : NdkBaseModel, INdkUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
