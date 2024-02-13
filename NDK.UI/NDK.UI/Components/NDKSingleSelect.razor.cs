using Microsoft.AspNetCore.Components;
using NDK.Core.Utility;
using NDK.UI.Components.Base;
using NDK.UI.Components.Common;
using NDK.UI.Models;

namespace NDK.UI.Components
{
    public partial class NDKSingleSelect<TValue> : BaseSelect<TValue> where TValue : NDKFinderOutput
    {
        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public EventCallback<TValue?> ValueChanged { get; set; }

        private TValue? SelectedValue { get; set; }


        protected override async Task OnFilter(string filter)
        {
            await base.OnFilter(filter);

            if (base.RemoveSelectedDataFromList && SelectedValue is not null)
            {
                var item = VisibleSource.Where(x => x.Id == SelectedValue.Id).FirstOrDefault();
                if (item != null)
                {
                    VisibleSource?.Remove(item);
                }
            }
        }
        protected override async Task OnSelect(TValue item)
        {
            if (SelectedValue is not null)
            {
                if (!VisibleSource?.Contains(SelectedValue) ?? false)
                {
                    var data = VisibleSource?.ToList();
                    data?.Add(SelectedValue);

                    var ordered = data?.OrderBy(x => x.Id);

                    FillData(ordered);
                }
            }

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(item);
            }

            if (base.RemoveSelectedDataFromList)
            {
                VisibleSource?.Remove(item);
            }

            SelectedValue = item;

            ShowPopup = !ShowPopup;

            if (ClearInputOutput)
            {
                FilterInput = string.Empty;
                VisibleSource!.Clear();
                SetSearchText(FilterInput);
                Searching = true;
            }

            await Task.CompletedTask;
        }

        protected override async Task OnFetch()
        {
            await base.OnFetch();

            await HandleSelectedData();
        }

        private async Task HandleSelectedData()
        {
            if (Value is not null)
            {
                if (AllowInitialFetch)
                {
                    var recoveredItem = VisibleSource.Where(x => x.Id == Value.Id).FirstOrDefault();
                    if (recoveredItem is not null)
                    {
                        SelectedValue = recoveredItem;

                        if (ValueChanged.HasDelegate)
                        {
                            await ValueChanged.InvokeAsync(recoveredItem);
                        }

                        if (RemoveSelectedDataFromList)
                        {
                            VisibleSource.Remove(recoveredItem);
                        }
                    }
                }
                else
                {
                    SelectedValue = Value;
                }
               
            }

            await Task.CompletedTask;
        }
        protected override async Task OnRemoveItem(TValue item)
        {
            if (ValueChanged.HasDelegate)
            {
                SelectedValue = null;
                await ValueChanged.InvokeAsync(null);
            }

            if (!VisibleSource?.Contains(item) ?? false)
            {
                var data = VisibleSource?.ToList();
                data?.Add(item);

                var ordered = data?.OrderBy(x => x.Id);

                FillData(ordered);
            }

        }


    }
}