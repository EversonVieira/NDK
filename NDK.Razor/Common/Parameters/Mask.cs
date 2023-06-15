using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Razor.Common.Parameters
{
    public class NdkMask
    {
        public int Code { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public NdkMaskType? Type { get; set; }

    }

    public enum NdkMaskType
    {
        OnlyNumber = 0,
        OnlyChar= 1,
        AlfaNumeric= 2,
    }

    public class NdkMaskOptions
    {
        public bool KeepSpecialChars { get; set; } = false;

    }
}
