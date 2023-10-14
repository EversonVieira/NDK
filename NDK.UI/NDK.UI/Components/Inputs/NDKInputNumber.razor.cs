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

        private const char THOUSANDS_CONST = ',';
        private const char DECIMAL_CONST = '.';

        [Parameter]
        public decimal? Max { get; set; }

        [Parameter]
        public decimal? Min { get; set; }

        [Parameter]
        public string? Prefix { get; set; }

        [Parameter]
        public string? Suffix { get; set;}

        [Parameter]
        public char Thousands { get; set; } = THOUSANDS_CONST;

        [Parameter]
        public char Decimal { get; set; } = DECIMAL_CONST;

        [Parameter]
        public int? Scale {  get; set; }



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
                bool srcValueChanged = false;
                if (args != null)
                {
                    string? value = (string?)args.Value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (!string.IsNullOrWhiteSpace(Prefix))
                        {
                            value = value.Replace(Prefix, "");
                        }

                        if (!string.IsNullOrWhiteSpace(Suffix))
                        {
                            value = value.Replace(Suffix, "");
                        }

                        if (value.Length == 1)
                        {
                            if (value.StartsWith("-"))
                            {
                                return;
                            }

                            if (value.StartsWith(".") || value.StartsWith(","))
                            {
                                value = $"0";
                                srcValueChanged = true;
                            }
                        }
                        else
                        {
                            if (value.StartsWith(".") || value.StartsWith(","))
                            {
                                value = $"0{value[0]}{value.Substring(1)}";
                                srcValueChanged = true;
                            }
                        }

                        if (Scale.HasValue)
                        {
                            int index = value.IndexOf(this.Decimal.ToString());
                            if ((value.Length - index) > Scale && index > -1)
                            {
                                srcValueChanged = true;
                                value = value.Substring(0, index + Scale.Value + 1);
                            }
                        }
                      

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


                    CurrentValueAsString = value;
                    if (srcValueChanged)
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

            if (value.Count(x => x == '-') > 1 ||
                value.Count(x => x == ',') > 1 ||
                value.Count(x => x == '.') > 0)
            {
                return false;
            }

            if (value.IndexOf("-") > 0)
            {
                return false;
            }



            for (int i = 0; i < value.Length; i++)
            {
                int charCode = value[i];

                if ((charCode < 48 || charCode > 57) && charCode != this.Decimal && charCode != 45)
                {
                    return false;
                }
            }

            return true;

        }

        protected override string? FormatValueAsString(TValue? value)
        {
            string? parcialResult = value switch
            {
                null => null,
                int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
                long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
                short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
                float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
                double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
                decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
                _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}")
            };

            if (string.IsNullOrWhiteSpace(parcialResult))
            {
                return parcialResult;
            }

            if (this.Thousands != THOUSANDS_CONST || this.Decimal != DECIMAL_CONST)
            {
                parcialResult = parcialResult.Replace(THOUSANDS_CONST.ToString(), nameof(THOUSANDS_CONST)).Replace(DECIMAL_CONST.ToString(), nameof(DECIMAL_CONST));
                parcialResult = parcialResult.Replace(nameof(THOUSANDS_CONST), this.Thousands.ToString()).Replace(nameof(DECIMAL_CONST), this.Decimal.ToString());
            }

            return parcialResult;
        }
    }
}