using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NDK.UI.Components.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

namespace NDK.UI.Components.Inputs
{
    public partial class NDKInputNumber<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : NDKBaseInput<TValue>
        where TValue : INumber<TValue>
    {
        [Parameter]
        public decimal? Max { get; set; }

        [Parameter]
        public decimal? Min { get; set; }

        protected static readonly string _stepAttributeValue = GetStepAttributeValue();

        private static string GetStepAttributeValue()
        {
            // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
            // of it for us. We will only get asked to parse the T for nonempty inputs.
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(short) ||
                targetType == typeof(float) ||
                targetType == typeof(double) ||
                targetType == typeof(decimal))
            {
                return "any";
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
            }
        }
        protected async Task Set(ChangeEventArgs? args)
        {
            await using (var commonJsInterop = new CommonJsInterop(Js))
            {
                if (args != null)
                {
                    string? value = (string?)args.Value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (!VerifyInputIntegrity(value))
                        {
                            CurrentValueAsString = CurrentValueAsString;
                            await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                            return;

                        }
                        TValue valueAsNum = (TValue)Convert.ChangeType(value, typeof(TValue));

                        if (Max.HasValue)
                        {
                            TValue maxAsType = (TValue)Convert.ChangeType(Max, typeof(TValue));
                            if (valueAsNum.CompareTo(maxAsType) > 0)
                            {
                                CurrentValueAsString = CurrentValueAsString;
                                await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                                return;
                            }
                        }

                        
                        if (Min.HasValue)
                        {
                            TValue minAsType = (TValue)Convert.ChangeType(Min, typeof(TValue));
                            if (valueAsNum.CompareTo(minAsType) < 0)
                            {
                                CurrentValueAsString = CurrentValueAsString;
                                await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                                return;
                            }
                        }

                       
                    }


                    CurrentValueAsString = (string?)args.Value;
                    await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);


                }
            }
        }

        protected override TValue? ParseValueFromString(string value)
        {
            if (!VerifyInputIntegrity(value))
            {
                return CurrentValue;
            }


            return base.ParseValueFromString(value);

        }

        protected override bool VerifyInputIntegrity(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            int charCode = value[0];

            if (charCode < 48 || 
                charCode > 57 )
            {
                if (charCode == 45)
                    return true;

                return false;
            }

            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out TValue? vlr))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected override string? FormatValueAsString(TValue? value)
        {
            // Avoiding a cast to IFormattable to avoid boxing.
            switch (value)
            {
                case null:
                    return null;

                case int @int:
                    return BindConverter.FormatValue(@int, CultureInfo.InvariantCulture);

                case long @long:
                    return BindConverter.FormatValue(@long, CultureInfo.InvariantCulture);

                case short @short:
                    return BindConverter.FormatValue(@short, CultureInfo.InvariantCulture);

                case float @float:
                    return BindConverter.FormatValue(@float, CultureInfo.InvariantCulture);

                case double @double:
                    return BindConverter.FormatValue(@double, CultureInfo.InvariantCulture);

                case decimal @decimal:
                    return BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture);

                default:
                    throw new InvalidOperationException($"Unsupported type {value.GetType()}");
            }
        }
    }
}