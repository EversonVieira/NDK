using Microsoft.AspNetCore.Components;

namespace NDK.UI.Components.Helpers
{
    public partial class NDKShow
    {
        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? HiddenClass { get; set; }

        [Parameter]
        public string? VisibleClass { get; set; }

        protected string GetClass()
        {

            if (Visible)
            {
                return VisibleClass is null ? string.Empty:VisibleClass;
            }

            return HiddenClass is null ? "hidden":HiddenClass;
        }
    }
}