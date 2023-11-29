using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using NDK.UI.Components.Forms.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NDK.UI.Components.Forms
{
    public partial class NDKForm<TModel> : NDKBaseComponent
    {
        [Parameter]
        public RenderFragment<TModel>? Sections { get; set; }

        [Parameter]
        public RenderFragment<TModel>? ChildContent { get; set; }

        [Parameter, NotNull]
        public TModel? Model { get; set; }

        protected FormData<TModel>? Data { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            Data = new FormData<TModel>()
            {
                Model = Model
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }


       
    }
}