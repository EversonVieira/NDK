using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Extensions
{
    public static class RenderTreeBuilderExtensions
    {
        public static void AddAttributeIfNotNullOrEmpty(this RenderTreeBuilder builder, int sequence, string name, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            builder.AddAttribute(sequence, name, value);
        }
    }
}
