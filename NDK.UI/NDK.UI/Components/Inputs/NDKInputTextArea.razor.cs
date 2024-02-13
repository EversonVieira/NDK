using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;

namespace NDK.UI.Components.Inputs
{
    public partial class NDKInputTextArea: NDKBaseInput<string?>
    {
        [Parameter]
        public int? MaxLength { get; set; }

        [Parameter]
        public int? MinLength { get; set; }

        protected async Task Set(ChangeEventArgs? args)
        {
            await using (var commonJsInterop = new CommonJsInterop(Js!))
            {
                if (args != null)
                {
                    string? value = (string?)args.Value;

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        CurrentValueAsString = default(string?);
                        await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                    }
                    else if (value.Length > MaxLength)
                    {
                        CurrentValueAsString = CurrentValueAsString;
                        await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                    }
                    else if (value.Length < MinLength)
                    {
                        CurrentValueAsString = CurrentValueAsString;
                        await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                    }
                    else
                    {
                        CurrentValueAsString = (string?)args.Value;
                    }


                }
            }
        }
    }
}