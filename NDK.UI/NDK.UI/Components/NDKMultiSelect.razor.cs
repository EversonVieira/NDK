using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Base;
using NDK.UI.Components.Common;
using NDK.UI.Models;
using System.Collections.ObjectModel;

namespace NDK.UI.Components
{
    public partial class NDKMultiSelect<T> : BaseSelect<T> where T : NDKFinderOutput
    {
        [Parameter]
        public List<T>? Value { get; set; }

        [Parameter]
        public EventCallback<List<T>?> ValueChanged { get; set; }

        private ObservableCollection<T> SelectedData { get; set; } = new ObservableCollection<T>();

      

        protected override async Task OnSelect(T item)
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

            await Task.CompletedTask;
        }

        protected override async Task OnRemoveItem(T item)
        {
            SelectedData.Remove(item);

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(SelectedData.ToList());
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