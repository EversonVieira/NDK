﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace NDK.UI.Components.Common
{
    public abstract class NDKBaseInput<TValue> : ComponentBase
    {
        [Inject]
        protected IJSRuntime? Js { get; set; }

        public ElementReference Element { get; protected set; }

        [Parameter, EditorRequired]
        public string? Id { get; set; }

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public string? Description { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Style { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public bool IsRequired { get; set; }

        [Parameter]
        public bool ApplyInputValidation { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public EventCallback<TValue?> ValueChanged { get; set; }

        [Parameter]
        public Expression<Func<TValue>>? ValueExpression { get; set; }


        protected virtual string? FormatValueAsString(TValue? value)
            => value?.ToString();

        protected virtual TValue? ParseValueFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            if (!VerifyInputIntegrity(value))
            {
                return CurrentValue;
            }

            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }


        protected string? CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set => CurrentValue = ParseValueFromString(value!);
        }

        protected TValue? CurrentValue
        {
            get => Value;
            set
            {
                Value = value;
                _ = ValueChanged.InvokeAsync(Value);
                
            }
        }

        public virtual bool IsValid()
        {
            if (IsRequired)
            {
                return string.IsNullOrEmpty(CurrentValueAsString);
            }

            return true;
        }

        protected virtual bool VerifyInputIntegrity(string? value) => true;
  

    }


}