using Microsoft.AspNetCore.Components;

namespace NDK.UI.Components
{
    public partial class NDKHeader
    {
        [Parameter]
        public RenderFragment? LeftContent { get; set; }

        [Parameter] 
        public RenderFragment? RightContent { get; set; }

        [Parameter]
        public RenderFragment? MiddleContent { get; set; }
    }
}