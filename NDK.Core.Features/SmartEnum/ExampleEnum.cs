using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Features.SmartEnum
{
    public class ExampleEnum : NDKSmartEnum<ExampleEnum>
    {
        public static readonly ExampleEnum normal = new(1, nameof(normal));
        public static readonly ExampleEnum hard = new(1, nameof(hard));
        public static readonly ExampleEnum easy = new(1, nameof(easy));

        private ExampleEnum(int value, string name) : base(value, name)
        {
        }
    }
}
