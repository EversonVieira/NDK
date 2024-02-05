using Microsoft.AspNetCore.Components;

namespace NDK.UI.Components.Helpers
{
    public partial class NDKForeach<T>
    {
        [Parameter] 
        public RenderFragment<T>? ByItem { get; set; }

        [Parameter]
        public RenderFragment<(T Value, int Index)>? ByItemAndIndex { get; set; }

        [Parameter, EditorRequired]
        public IEnumerable<T> DataSource { get; set; }
    }
}