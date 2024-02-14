using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        public string? Title { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Style { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public string? Height { get; set; }

        [Parameter]
        public string? Width { get; set; }

        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        protected bool IsRendered = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender )
            {
                IsRendered = true;
            }
        }

        protected virtual string? GetStyle()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Height))
            {
                stringBuilder.Append($"height:{Height};");
            }

            if (!string.IsNullOrWhiteSpace(Width))
            {
                stringBuilder.Append($"width:{Width};");
            }

            stringBuilder.Append(Style);

            return stringBuilder.ToString();
        }
    }


}
