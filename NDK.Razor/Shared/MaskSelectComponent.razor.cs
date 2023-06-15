using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using NDK.Razor.Common.Parameters;

namespace NDK.Razor.Shared
{
    public partial class MaskSelectComponent
    {
        [Parameter]
        public List<NdkMask>? MaskCollection { get; set; }

        [Parameter]
        public NdkMask? SelectedMask { get; set; }

        [Parameter]
        public EventCallback<NdkMask?> SelectedMaskChanged { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (MaskCollection != null && MaskCollection.Any()) 
                {
                    this.SelectedMask = MaskCollection.First();
                    StateHasChanged();
                }
            }

        }

        public async Task SelectMask(NdkMask? mask)
        {
            this.SelectedMask = mask;
            await this.SelectedMaskChanged.InvokeAsync(mask);
        }
    }
}