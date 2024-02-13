using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Common
{
    public abstract class NDKBaseComponent : ComponentBase
    {
        [Parameter]
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public string? Description { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? StyleClass { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        protected bool IsRendered = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender )
            {
                IsRendered = true;
            }
        }
    }


}
