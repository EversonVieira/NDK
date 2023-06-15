using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NDK.Razor.Common.Parameters;
using NDK.Razor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Razor.Inputs.Base
{
    internal class NdkBaseInputText : NdkBaseInput<string>
    {
        [Parameter]
        public int MaxSize { get; set; }

        [Parameter]
        public int MinSize { get; set; }

     

        public override void OnAfterValueChange(string newValue)
        {
            //List<(int minKey, int maxKey)> asciCodes = new List<(int minKey, int maxKey)>
            //{
            //    new(48, 57),
            //    new(65, 90),
            //    new(97, 122)
            //};

        }

        public override void OnBeforeValueChange(string newValue)
        {

        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //if (MaskCollection != null && MaskCollection.Any())
            //{
            //    builder.OpenComponent<MaskSelectComponent>(0);
            //}
            base.BuildRenderTree(builder);
        }


    }
}
