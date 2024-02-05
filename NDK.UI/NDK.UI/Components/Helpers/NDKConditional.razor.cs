using Microsoft.AspNetCore.Components;

namespace NDK.UI.Components.Helpers
{
    public partial class NDKConditional
    {
        [Parameter]
        public bool If { get; set; }

        [Parameter]
        public RenderFragment? Then { get; set; }

        [Parameter]
        public RenderFragment? Else {  get; set; }
    }
}