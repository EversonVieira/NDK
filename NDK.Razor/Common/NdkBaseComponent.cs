using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Razor.Common
{
    public class NdkBaseComponent:ComponentBase
    {
        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-","");

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Style { get; set; }
    }
}
