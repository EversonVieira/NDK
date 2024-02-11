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
        public string? Suffix { get; set; }

        [Parameter]
        public char Thousands { get; set; } = THOUSANDS_CONST;

        [Parameter]
        public char Decimal { get; set; } = DECIMAL_CONST;

        [Parameter]
        public int? Scale { get; set; }



        protected static readonly string _stepAttributeValue = GetStepAttributeValue();

        protected string GetMaxLength()
        {
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            if (targetType == typeof(int))
            {
                return int.MaxValue.ToString().Length.ToString();
            }

            if (targetType == typeof(long))
            {
                return long.MaxValue.ToString().Length.ToString();
            }

            if (targetType == typeof(short))
            {
                return short.MaxValue.ToString().Length.ToString();
            }

            if (targetType == typeof(float))
            {
                return float.MaxValue.ToString().Length.ToString();
            }

            if (targetType == typeof(double))
            {
                return double.MaxValue.ToString().Length.ToString();
            }

            if (targetType == typeof(decimal))
            {
                return decimal.MaxValue.ToString().Length.ToString();
            }

            return string.Empty;

        }

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

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        value = "0";
                        await commonJsInterop.SetInputValue(Element, value!);
                        return;
                    }

                    if (value.StartsWith($"{(Min.HasValue ? Min.ToString() : "0")}", StringComparison.Ordinal))
                    {
                        value = value.Substring(1);
                    }

                    if (Scale.HasValue && value.Contains(Decimal))
                    {
                        int counter = value.Length - value.IndexOf(Decimal);
                        if (counter > Scale.Value + 1)
                        {
                            value = value.Substring(0, value.Length - 1);
                            await commonJsInterop.SetInputValue(Element, value!);
                            return;
                        }
                    }

                    if (!VerifyInputIntegrity(value))
                    {
                        CurrentValueAsString = CurrentValueAsString;
                        await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                        return;
                    }

                    if (value.Length == 1)
                    {
                        if (value.StartsWith("-"))
                        {
                            return;
                        }

                        if (value.StartsWith(Thousands) || value.StartsWith(Decimal))
                        {
                            value = $"{(Min.HasValue ? Min.ToString() : "0")}";
                            srcValueChanged = true;
                        }
                    }


                    value = NormalizeValue(value);

                    if (!BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out var tmpvalue))
                    {
                        if (value.Length > 0)
                        {
                            value = value.Substring(0, value.Length - 1);
                        }
                    }




                    CurrentValueAsString = value;
                    if (srcValueChanged)
                        await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                }
            }
        }

        private string NormalizeValue(string value)
        {
            value = value.Replace(Thousands.ToString(), nameof(Thousands)).Replace(Decimal.ToString(), nameof(Decimal));
            value = value.Replace(nameof(Thousands), THOUSANDS_CONST.ToString()).Replace(nameof(Decimal), DECIMAL_CONST.ToString());
            return value;

        }
        protected async Task SetChange(ChangeEventArgs args)
        {
            await using (var commonJsInterop = new CommonJsInterop(Js))
            {
                bool srcValueChanged = false;
                if (args != null)
                {
                    string? value = (string?)args.Value;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (value.EndsWith(Thousands.ToString()) || value.EndsWith(Decimal.ToString()))
                        {
                            srcValueChanged = true;
                            value = value.Substring(0, value.Length - 1);
                        }

                        if (value.Length == 1)
                        {
                            if ((value.StartsWith("-") || value.StartsWith(".") || value.StartsWith(",")))
                            {
                                value = $"{(Min.HasValue ? Min.ToString() : "0")}";
                                srcValueChanged = true;
                            }
                        }
                        else if (value.Length > 1)
                        {
                            if (value.StartsWith(Thousands.ToString()) || value.StartsWith(Decimal.ToString()))
                            {
                                value = $"{{(Min.HasValue ? Min.ToString():\"0\"{value[0]}{value.Substring(1)}";
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

                        value = NormalizeValue(value);

                        if (!BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out var tmpvalue))
                        {
                            value = value.Substring(0, value.Length - 1);
                        }

                        TValue valueAsNum = (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.GetCultureInfo("en-us"));

                        if (!await ValidateMaxMin(valueAsNum, commonJsInterop)) return;


                    }

                    CurrentValueAsString = value;
                    if (srcValueChanged)
                        await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                }
            }


            await Task.CompletedTask;
        }

        private async Task<bool> ValidateMax(TValue valueAsNum, CommonJsInterop commonJsInterop)
        {
            if (Max.HasValue)
            {
                TValue maxAsType = (TValue)Convert.ChangeType(Max, typeof(TValue), CultureInfo.GetCultureInfo("en-us"));
                if (valueAsNum.CompareTo(maxAsType) > 0)
                {
                    CurrentValueAsString = Max.ToString();
                    await commonJsInterop.SetInputValue(Element, Max.ToString()!);
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> ValidateMin(TValue valueAsNum, CommonJsInterop commonJsInterop)
        {
            if (Min.HasValue)
            {
                TValue minAsType = (TValue)Convert.ChangeType(Min, typeof(TValue), CultureInfo.GetCultureInfo("en-us"));
                if (valueAsNum.CompareTo(minAsType) < 0)
                {
                    CurrentValueAsString = Min.ToString();
                    await commonJsInterop.SetInputValue(Element, Min.ToString()!);
                    return false;
                }
            }

            return true;
        }
        private async Task<bool> ValidateMaxMin(TValue valueAsNum, CommonJsInterop commonJsInterop) =>
           await ValidateMax(valueAsNum, commonJsInterop) && await ValidateMin(valueAsNum, commonJsInterop);



        protected override TValue? ParseValueFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            return (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.GetCultureInfo("en-us"));
        }

        protected override bool VerifyInputIntegrity(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (value.Count(x => x == '-') > 1 ||
                value.Count(x => x == Decimal) > 1 ||
                value.Count(x => x == Thousands) > 0 ||
                (!Scale.HasValue &&
                (value.Count(x => x == Decimal) > 0 ||
                value.Count(x => x == Thousands) > 0)))
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
                int @int => BindConverter.FormatValue(@int, CultureInfo.GetCultureInfo("en-us")),
                long @long => BindConverter.FormatValue(@long, CultureInfo.GetCultureInfo("en-us")),
                short @short => BindConverter.FormatValue(@short, CultureInfo.GetCultureInfo("en-us")),
                float @float => BindConverter.FormatValue(@float, CultureInfo.GetCultureInfo("en-us")),
                double @double => BindConverter.FormatValue(@double, CultureInfo.GetCultureInfo("en-us")),
                decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.GetCultureInfo("en-us")),
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