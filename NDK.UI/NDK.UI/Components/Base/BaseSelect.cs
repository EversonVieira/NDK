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
        public INDKTextFinder<T>? Finder { get; set; }

        [Parameter]
        public bool InMemoryFilter { get; set; }

        [Parameter]
        public bool RemoveSelectedDataFromList { get; set; } = true;

        [Parameter]
        public Func<T?, string>? TextExpression { get; set; }

        [Parameter]
        public int MinFilterLength { get; set; } = 3;

        [Parameter]
        public int SearchWaiterMS { get; set; } = 500;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ObservableCollection<T> _source;
        protected ObservableCollection<T> VisibleSource { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private NDKWaiter _waiter = new NDKWaiter();

        protected async Task OnFilter(string filter)
        {

            if (InMemoryFilter)
            {
                var data = string.IsNullOrWhiteSpace(filter) ? _source : _source?.Where(x => GetText(x)?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ?? false);
                FillData(data);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    await OnFetch();
                    return;
                }


                if (filter.Length < MinFilterLength)
                {
                    return;
                }

                var data = await Finder!.FindAsync(filter);

                FillData(data);
              
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

                await OnFetch();

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
    }
}
