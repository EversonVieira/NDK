using Microsoft.AspNetCore.Components;

namespace NDK.UI.Components
{
    public partial class NDKWindow
    {
        [Parameter]
        public RenderFragment? Header { get; set; }

        [Parameter]
        public RenderFragment? Body { get; set; }

        [Parameter]
        public RenderFragment? Footer { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }
    }
}