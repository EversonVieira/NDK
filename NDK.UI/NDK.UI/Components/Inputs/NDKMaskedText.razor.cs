using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using System.Text;

namespace NDK.UI.Components.Inputs
{
    public partial class NDKMaskedText : NDKBaseInput<string?>
    {
        [Parameter, EditorRequired]
        public string? Mask { get; set; }

        private List<(int pos, char c)> replacableList = new List<(int pos, char c)>();

        [Parameter]
        public bool MatchCase { get; set; } = true;

        protected async Task Set(ChangeEventArgs? args)
        {
            await using (var commonJsInterop = new CommonJsInterop(Js))
            {
                if (args != null)
                {
                    string? value = (string?)args.Value;

                    if (string.IsNullOrWhiteSpace(Mask))
                    {
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            CurrentValueAsString = default(string?);
                            await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                        }
                        else
                        {
                            CurrentValueAsString = (string?)args.Value;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            CurrentValueAsString = await WithMask(default(string?) ?? string.Empty);
                            await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);

                        }
                        else
                        {
                            CurrentValueAsString = await WithMask((string?)args.Value ?? string.Empty);
                            await commonJsInterop.SetInputValue(Element, CurrentValueAsString!);
                           
                        }
                    }
                }
            }

            Console.WriteLine(CurrentValueAsString);


        }
      
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (!string.IsNullOrWhiteSpace(Mask))
            {
                for (int i = 0; i < Mask.Length; i++)
                {
                    if ((Mask[i] >= '0' && Mask[i] <= '9') ||
                        (Mask[i] >= 'a' && Mask[i] <= 'z') ||
                        (Mask[i] >= 'A' && Mask[i] <= 'Z'))
                    {
                        replacableList.Add((i, Mask[i]));
                    }
                }
            }
            await Task.CompletedTask;

        }

        private async Task<string> WithMask(string input)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < input.Length; i++)
            {
                if (replacableList.Exists((data) => data.pos == i))
                {
                    if (await AcceptChartType(i, input[i]))
                    {
                        sb.Append(input[i]);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Mask) && Mask.Length - 1 >= i)
                    {
                        sb.Append(Mask[i]);
                    }

                    if (replacableList.Exists((data) => data.pos == i+1))
                    {
                        if (await AcceptChartType(i+1, input[i]))
                        {
                            sb.Append(input[i]);
                        }
                    }
                }
            }


            await Task.CompletedTask;

            return sb.ToString();
        }

        private async Task<bool> AcceptChartType(int pos, char c)
        {
            var reg = replacableList.Find(data => data.pos == pos); 

            if (string.IsNullOrWhiteSpace(Mask))
            {
                return true;
            }

            if (reg == default || (Mask.Length - 1 < pos))
            {
                return false;
            }

            if (MatchCase)
            {
                if (await GetCharType(c) == await GetCharType(Mask[pos]))
                {
                    return true;
                }
            }
            else
            {
                bool isLetterInput = (await GetCharType(c) is CharType.LowerCaseLetters or CharType.UpperCaseLetters); 
                bool isLetterMask = (await GetCharType(Mask[pos]) is CharType.LowerCaseLetters or CharType.UpperCaseLetters); 

                if (isLetterInput && isLetterMask)
                {
                    return true;
                }
            }
          

            await Task.CompletedTask;

            return false;
        }

        private async Task<CharType> GetCharType(char c)
        {
            if (c >= 'a' && c<= 'z')
            {
                return CharType.LowerCaseLetters;
            }

            if (c >= 'A' && c<= 'Z')
            {
                return CharType.UpperCaseLetters;
            }

            if (c >= '0' && c<= '9')
            {
                return CharType.Number;
            }

            await Task.CompletedTask;

            return CharType.None;
        }

        private enum CharType
        {
            None,
            Number,
            LowerCaseLetters,
            UpperCaseLetters
        }
    }
}