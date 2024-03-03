using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NDK.UI.Components.Common;
using NDK.UI.Components.Inputs;
using NDK.UI.Components.Interfaces;
using NDK.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Base
{
    public abstract class BaseSelect<T> : NDKBaseComponent
        where T : NDKFinderOutput
    {
        /// <summary>
        /// Interface that should be provided to do the Fetch
        /// </summary>
        [Parameter]
        public INDKTextFinder<T>? Finder { get; set; }

        /// <summary>
        /// Will filter in memory if true
        /// If True, allows the component to do the fetch without providing any parameter
        /// If false, it will use your provided values to set the SelectedData, this means the input can't be just the Id.
        /// </summary>
        [Parameter]
        public bool InMemoryFilter { get; set; }

        /// <summary>
        /// Will keep the Selected data outside the DataSource
        /// </summary>
        [Parameter]
        public bool RemoveSelectedDataFromList { get; set; } = true;

        /// <summary>
        /// If provided, will be used instead of just the Text property to show and filter on the display.
        /// </summary>
        [Parameter]
        public Func<T, string>? TextExpression { get; set; }

        /// <summary>
        /// Min input lenght when attempting to filter, applys only on NOT in memory filter.
        /// </summary>
        [Parameter]
        public int MinFilterLength { get; set; } = 3;

        /// <summary>
        /// The waiter time to debounce the task
        /// </summary>
        [Parameter]
        public int DebounceTimeStampMS { get; set; } = 500;

        /// <summary>
        /// The Searching Text "Searching..."
        /// </summary>
        [Parameter]
        public string SearchingText { get; set; } = "Searching...";

        /// <summary>
        /// The No Data Found Text
        /// </summary>
        [Parameter]
        public string NoDataFoundText { get; set; } = "No Data Found";

        /// <summary>
        /// Provide a template to "Type more {0} to find"
        /// Use {0} to provide the remaining number of characters to find.
        /// </summary>
        [Parameter]
        public string TypeMoreStringTemplate { get; set; } = "Type more {0} to find";

        /// <summary>
        /// Set true if you want the VisibleSource and the Input to be cleared when Selecting a item
        /// Only Applicable if not In Memory Filter
        /// </summary>
        [Parameter]
        public bool ClearInputOutput { get; set; } = true;

        protected string? SearchText { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ObservableCollection<T> _source;
        protected ObservableCollection<T> VisibleSource { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected bool ShowPopup { get; set; }
       
        protected bool Searching {  get; set; }

        protected string? FilterInput { get; set; }

        protected NDKInputText? InputRef { get; set; }

        protected async Task OnFocus()
        {
            if (JSRuntime is not null && InputRef is not null)
            {
                await using (var commonJsRuntime = new CommonJsInterop(JSRuntime))
                {
                    await commonJsRuntime.SetFocus(InputRef.Element);
                }
            }

            ShowPopup = !ShowPopup;
           
            await Task.CompletedTask;
        }

        protected virtual async Task OnKeyPress(KeyboardEventArgs args)
        {

            await Task.CompletedTask;
        }
        protected virtual async Task OnFilter(string filter)
        {
            SetSearchText(filter);

            FilterInput = filter;

            if (InMemoryFilter)
            {
                var data = string.IsNullOrWhiteSpace(filter) ? _source : _source?.Where(x => GetText(x)?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ?? false);
                FillData(data);
            }
            else
            {
                Searching = true;

                if (string.IsNullOrWhiteSpace(filter))
                {
                    if (MinFilterLength == 0)
                    {
                        await OnFetch();
                    }

                    return;
                }



                if (filter.Length < MinFilterLength)
                {
                    return;
                }


                await NDKWaiter.Debounce("ndk_debounce_on_filter",DebounceTimeStampMS, async () =>
                {
                    var data = await Finder!.FindAsync(filter);

                    FillData(data);

                });

                Searching = false;

            }

            await Task.CompletedTask;

        }

        protected abstract Task OnSelect(T item);


        /// <summary>
        /// Happens when remove the selected item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract Task OnRemoveItem(T item);


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _source = new ObservableCollection<T>();
                VisibleSource = InMemoryFilter ? new ObservableCollection<T>() : _source;

                if (InMemoryFilter)
                {
                    await OnFetch();
                }
                else
                {
                    Searching = true;
                    SetSearchText(string.Empty);
                }

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected virtual async Task OnFetch()
        {
            if (Finder is null)
            {
                throw new ArgumentNullException($"{nameof(Finder)} should be provided");
            }

            var data = await Finder.FindAsync();

            VisibleSource.Clear();
            data.ForEach(x =>
            {
                _source?.Add(x);
                if (InMemoryFilter)
                {
                    VisibleSource.Add(x);
                }
            });


            await Task.CompletedTask;
        }

        protected void FillData(IEnumerable<T>? data)
        {
            VisibleSource.Clear();

            if (data is null)
            {
                return;
            }

            foreach (var item in data)
            {
                VisibleSource.Add(item);
            }

        }



        protected virtual string? GetText(T? item)
        {
            if (TextExpression is not null)
            {
                return TextExpression(item);
            }

            if (string.IsNullOrWhiteSpace(item?.Text))
            {
                return string.Empty;
            }

            return item?.Text!;
        }

        protected virtual void SetSearchText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                SearchText = string.Format(TypeMoreStringTemplate, MinFilterLength);
                return;
            }

            if (input.Length < MinFilterLength)
            {
                SearchText = string.Format(TypeMoreStringTemplate, MinFilterLength - input.Length);
                return;
            }

            SearchText = SearchingText;

        }
    }
}
