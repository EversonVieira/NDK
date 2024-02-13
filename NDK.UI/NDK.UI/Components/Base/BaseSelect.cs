using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
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
        [Parameter]
        public INDKITextFinder<T>? Finder { get; set; }

        [Parameter]
        public bool InMemoryFilter { get; set; }

        [Parameter]
        public bool RemoveSelectedDataFromList { get; set; }

        [Parameter]
        public Func<T, string>? TextExpression { get; set; }

        [Parameter]
        public int MinFilterLength { get; set; } = 3;

        [Parameter]
        public int SearchWaiterMS { get; set; }

        private ObservableCollection<T>? _source { get; set; }
        protected ObservableCollection<T>? _visibleSource { get; set; }
        private DateTime? _filterDate { get; set; }

        private NDKWaiter _waiter = new NDKWaiter();

        protected async Task OnFilter(string filter)
        {
            if (filter.Length < MinFilterLength)
            {
                return;
            }

            _filterDate = DateTime.Now;


            if (InMemoryFilter)
            {
                var data = _source?.Where(x => GetText(x)?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ?? false);
                FillData(data);
            }
            else
            {
                await _waiter.Wait(() =>
                {
                    var ts = DateTime.Now - _filterDate;
                    return ts.HasValue && ts.Value >= TimeSpan.FromMilliseconds(SearchWaiterMS);
                }, () =>
                {
                    var data = Finder?.Find(filter);

                    FillData(data);
                });
            }

            await Task.CompletedTask;
        }

        protected abstract Task OnSelect(T item);

        /// <summary>
        /// Happens when the Flag RemoveSelectedDataFromList is true
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract Task OnRemoveSelectedData(T item);

        /// <summary>
        /// Happens when remove the selected item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract Task OnRemoveItem(T item);

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            _source = new ObservableCollection<T>();
            _visibleSource = InMemoryFilter ? new ObservableCollection<T>() : _source;

            await OnFetch();
        }

        protected virtual async Task OnFetch()
        {
            if (Finder is null)
            {
                throw new ArgumentNullException($"{nameof(Finder)} should be provided");
            }

            var data = Finder.Find();

            data.ForEach(x =>
            {
                _source?.Add(x);
                _visibleSource?.Add(x);
            });


            await Task.CompletedTask;
        }

        protected void FillData(IEnumerable<T>? data)
        {
            _visibleSource?.Clear();

            if (data is null)
            {
                return;
            }

            foreach (var item in data)
            {
                _visibleSource?.Add(item);
            }
        }

        protected virtual MarkupString? GetText(T item)
        {
            if (TextExpression is not null)
            {
                return (MarkupString?)TextExpression(item);
            }

            return (MarkupString?) item?.Text!;
        }
    }
}
