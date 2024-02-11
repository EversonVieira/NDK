using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NDK.UI.Components.Common;
using System.Globalization;
using System.Text;

namespace NDK.UI.Components.Inputs
{
    public partial class NDKInputDate : NDKBaseInput<DateTime?>
    {
        [Parameter]
        public DateFormatEnum DateFormat { get; set; } = DateFormatEnum.YMD;

        protected string DateFormatString { get; set; } = "yyyy/MM/dd";

        private List<int> ReplacableList { get; set; } = new List<int>();
        protected bool CalendarIsVisible { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            DateFormatString = DateFormat switch
            {
                DateFormatEnum.DMY => "dd/MM/yyyy",
                DateFormatEnum.MDY => "MM/dd/yyyy",
                _ => "yyyy/MM/dd",
            }; ;

            for (int i = 0; i < DateFormatString.Length; i++)
            {
                if (DateFormatString[i] != '/')
                    ReplacableList.Add(i);
            }

        }

        protected async Task ShowCalendar()
        {
            CalendarIsVisible = !CalendarIsVisible;
            await Task.CompletedTask;
        }

        protected async Task Set(ChangeEventArgs args)
        {
            await using (var commonJsInterop = new CommonJsInterop(Js))
            {
                string? value = (string?)args.Value;

                if (!VerifyInput(value))
                {
                    CurrentValueAsString = null;
                    await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                    return;
                }

                CurrentValueAsString = value;

                Console.WriteLine(CurrentValueAsString);
                Console.WriteLine(CurrentValue);
            }

            await Task.CompletedTask;
        }

        protected override string? FormatValueAsString(DateTime? value)
        {
            return value?.ToString(DateFormatString) ?? string.Empty;
        }
        protected override DateTime? ParseValueFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string[] dateParts = value.Split('/');
            int year = 0, month = 0, day = 0;

            if (DateFormat == DateFormatEnum.YMD)
            {
                year = Convert.ToInt32(dateParts[0]);
                month = Convert.ToInt32(dateParts[1]);
                day = Convert.ToInt32(dateParts[2]);
            }
            else if (DateFormat == DateFormatEnum.DMY)
            {
                day = Convert.ToInt32(dateParts[0]);
                month = Convert.ToInt32(dateParts[1]);
                year = Convert.ToInt32(dateParts[2]);

            }
            else if (DateFormat == DateFormatEnum.MDY)
            {
                month = Convert.ToInt32(dateParts[0]);
                day = Convert.ToInt32(dateParts[1]);
                year = Convert.ToInt32(dateParts[2]);

            }

            return new DateTime(year, month, day);
            
        }
        protected bool VerifyInput(string? value)
        {

            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            string[] dateParts = value.Split('/');

            if (dateParts.Length != 3) { return false; }

            int year = 0, month = 0, day = 0;

            if (DateFormat == DateFormatEnum.YMD)
            {
                year = Convert.ToInt32(dateParts[0]);
                month = Convert.ToInt32(dateParts[1]);
                day = Convert.ToInt32(dateParts[2]);
            }
            else if (DateFormat == DateFormatEnum.DMY)
            {
                day = Convert.ToInt32(dateParts[0]);
                month = Convert.ToInt32(dateParts[1]);
                year = Convert.ToInt32(dateParts[2]);

            }
            else if (DateFormat == DateFormatEnum.MDY)
            {
                month = Convert.ToInt32(dateParts[0]);
                day = Convert.ToInt32(dateParts[1]);
                year = Convert.ToInt32(dateParts[2]);

            }


            return IsDateProvidedValid(year, month, day);
        }

        private bool IsDateProvidedValid(int year, int month, int day)
        {
            return (year is >= 1800 and <= 9999) && (month is >= 1 and <= 12) && (day >= 1 && day <= DateTime.DaysInMonth(year, month));
        }

        protected async Task SetMask(ChangeEventArgs args)
        {

            await using (var commonJsInterop = new CommonJsInterop(Js))
            {
                string? value = (string?)args.Value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < value.Length; i++)
                {
                    if (i >= DateFormatString.Length)
                    {
                        await commonJsInterop.SetInputValue(Element, sb.ToString());
                        return;
                    }

                    if (ReplacableList.Contains(i))
                    {
                        if (value[i] >= '0' && value[i] <= '9')
                        {
                            if (DateFormatString[i] == '/')
                            {
                                sb.Append($@"/{value[i]}");
                            }
                            else
                            {
                                sb.Append(value[i]);
                            }
                        }
                    }
                    else
                    {
                        sb.Append('/');

                        if (value[i] >= '0' && value[i] <= '9')
                        {

                            sb.Append(value[i]);
                        }
                    }


                }

                await commonJsInterop.SetInputValue(Element, sb.ToString());
            }

        }


        public enum DateFormatEnum
        {
            DMY,
            MDY,
            YMD,
        }
    }
}