using Microsoft.AspNetCore.Components;
using NDK.Core.Utility;
using NDK.UI.Components.Base;
using NDK.UI.Components.Common;
using NDK.UI.Models;

namespace NDK.UI.Components
{
    public partial class NDKSingleSelect<T> : BaseSelect<T> where T: NDKFinderOutput
    {
        [Parameter]
        public T? Value { get; set; }

        [Parameter]
        public EventCallback<T?> ValueChanged { get; set; }

        protected override async Task OnRemoveSelectedData(T item)
        {
            if (!base.RemoveSelectedDataFromList || Value is null) return;

            _visibleSource?.Remove(item);

            await Task.CompletedTask;
        }

        protected override async Task OnSelect(T item)
        {
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(item);
            }

            await OnRemoveSelectedData(item);

            await Task.CompletedTask;
        }

        protected override async Task OnRemoveItem(T item)
        {
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(item);
            }

            if (!_visibleSource?.Contains(item) ?? false) 
            {
                var data = _visibleSource?.ToList();
                data?.Add(item);

                var ordered = data?.OrderBy(x => x.Id);

                FillData(ordered);
            }
        }
    }
}