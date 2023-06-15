using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NDK.Razor.Common;
using Microsoft.AspNetCore.Components.Rendering;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace NDK.Razor.Inputs.Base
{
    internal abstract class NdkBaseInput<TValue> : NdkBaseComponent
    {
        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        private string? _valueAsString = string.Empty;
        protected string? ValueAsString
        {
            get 
            { 
                return _valueAsString; 
            }
            set 
            { 
                _valueAsString = value;

                ValueChanged.InvokeAsync((TValue) Convert.ChangeType(value, typeof(TValue)));
            }
        }

        [Parameter]
        public EventCallback<TValue?> ValueChanged { get; set; }

        [Parameter]
        public string? DivClass { get; set; }

        [Parameter]
        public string? LabelClass { get; set; }

        [Parameter]
        public string? InputClass { get; set; }

        [Parameter]
        public string? InputDivClass { get; set; }

        public abstract void OnBeforeValueChange(TValue newValue);
        public abstract void OnAfterValueChange(TValue newValue);

        public string? Event { get; set; }
        public ElementReference Element { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", $"ndk-inp-div-class {DivClass}");

            builder.OpenElement(2, "label");
            builder.AddAttribute(3, "class", $"ndk-inp-label-class {LabelClass}");
            builder.AddContent(4, Label);
            builder.CloseElement();

            builder.OpenElement(5, "div");
            builder.AddAttribute(6, "class", $"ndk-inp-cnt-class {InputDivClass}");

            builder.OpenElement(7, "input");
            builder.AddAttribute(8, "type", GetInputType());
            builder.AddAttribute(9, "class", $"ndk-inp-class {InputClass}");
            builder.AddAttribute(10, "placeholder", Placeholder);

            builder.AddAttribute(11, Event is null ? "onchange":Event, EventCallback.Factory.CreateBinder(this, __valor => ValueAsString = __valor, ValueAsString));
            builder.AddAttribute(12, "value", BindConverter.FormatValue(ValueAsString));

            if (Disabled)
            {
                builder.AddAttribute(13, "disabled");
            }

            builder.AddElementReferenceCapture(13, (r) => Element = r);
            builder.CloseElement(); // fecha o input
            builder.CloseElement(); // fecha a div
            builder.CloseElement(); // fecha a div
        }

        public async Task FocusAsync()
        {
            await Element.FocusAsync();
        }

        private static string GetInputType()
        {
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            if (targetType == typeof(DateTime?) ||
                targetType == typeof(DateTime))
            {
                return "date";
            }

            return "text";
        }
    }
}
