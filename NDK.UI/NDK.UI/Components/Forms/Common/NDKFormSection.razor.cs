using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using System.Diagnostics.CodeAnalysis;

namespace NDK.UI.Components.Forms.Common
{

    public partial class NDKFormSection<TModel>:NDKBaseComponent
    {
        [CascadingParameter, NotNull]
        public FormData<TModel>? FormData { get; set; }

        [Parameter]
        public RenderFragment<TModel>? ChildContent { get; set; }

        [Parameter]
        public RenderFragment? SectionHeader { get; set; }

        [Parameter]
        public bool Collapsable { get; set; } = true;

        [Parameter]
        public bool StartExpanded { get; set; } = true;

        protected bool IsExpanded { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            IsExpanded = StartExpanded;
        }

        protected async Task ExpandCollapse()
        {
            IsExpanded = !IsExpanded;

            await Task.CompletedTask;
        }
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
    }
}