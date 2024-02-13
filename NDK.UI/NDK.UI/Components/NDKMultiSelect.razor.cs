using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Base;
using NDK.UI.Components.Common;
using NDK.UI.Models;
using System.Collections.ObjectModel;

namespace NDK.UI.Components
{
    public partial class NDKMultiSelect<TValue> : BaseSelect<TValue> where TValue : NDKFinderOutput
    {
        [Parameter]
        public List<TValue>? Value { get; set; }

        [Parameter]
        public EventCallback<List<TValue>?> ValueChanged { get; set; }

        private ObservableCollection<TValue> SelectedData { get; set; } = new ObservableCollection<TValue>();


        protected override async Task OnSelect(TValue item)
        {
            SelectedData.Add(item);

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(SelectedData.ToList());
            }

            if (base.RemoveSelectedDataFromList)
            {
                if (VisibleSource is not null)
                {
                    VisibleSource.Remove(item);
                }
            }

            ShowPopup = !ShowPopup;

            if (ClearInputOutput && !InMemoryFilter)
            {
                FilterInput = string.Empty;
                VisibleSource!.Clear();
                SetSearchText(FilterInput);
                Searching = true;
            }

            await Task.CompletedTask;
        }

        protected override async Task OnFilter(string filter)
        {
            SetSearchText(filter);

            await base.OnFilter(filter);

            if (RemoveSelectedDataFromList)
            {
                if (VisibleSource is not null)
                {
                    foreach (var item in SelectedData)
                    {
                        var recoveredItem = VisibleSource.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (recoveredItem is not null)
                        {
                            if (RemoveSelectedDataFromList)
                            {
                                VisibleSource.Remove(recoveredItem);
                            }
                        }
                    }
                }
            }

        }
        protected override async Task OnFetch()
        {
            await base.OnFetch();

            await HandleSelectedData();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender && !InMemoryFilter)
            {
                await HandleSelectedData();
            }
        }

        private async Task HandleSelectedData()
        {
            SelectedData.Clear();
            if (Value is not null)
            {
                foreach (var item in Value)
                {
                    if (InMemoryFilter)
                    {
                        var recoveredItem = VisibleSource.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (recoveredItem is not null)
                        {
                            SelectedData.Add(recoveredItem);

                            if (RemoveSelectedDataFromList)
                            {
                                VisibleSource.Remove(recoveredItem);
                            }
                        }
                    }
                    else
                    {
                        SelectedData.Add(item);
                    }
                   
                }
            }

            await Task.CompletedTask;
        }

        protected override async Task OnRemoveItem(TValue item)
        {
            SelectedData.Remove(item);

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(SelectedData.ToList());
            }

            if (!VisibleSource?.Contains(item) ?? false && InMemoryFilter)
            {
                var data = VisibleSource?.ToList();
                data?.Add(item);

                var ordered = data?.OrderBy(x => x.Id);

                FillData(ordered);
            }

        }
    }
}