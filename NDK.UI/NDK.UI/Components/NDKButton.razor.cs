using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;

namespace NDK.UI.Components
{
    public partial class NDKButton: NDKBaseInput<object>
    {

        [Parameter]
        public RenderFragment? ChildContent { get; set; }


        [Parameter]
        public EventCallback OnClick { get; set; }


        protected async Task ClickAsync()
        {

            if (!OnClick.HasDelegate) return;

            await OnClick.InvokeAsync();
        }
    }
}