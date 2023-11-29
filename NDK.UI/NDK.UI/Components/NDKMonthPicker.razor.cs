using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using System.Collections.ObjectModel;

namespace NDK.UI.Components
{
    public partial class NDKMonthPicker : NDKBaseInput<int>
    {
        [Parameter]
        public int MinimalMonth { get; set; } = 1800;

        [Parameter]
        public int MaximumMonth { get; set; } = 9999;

        [Parameter]
        public bool CurrentMonthAsDefaultValue { get; set; }

        [Parameter]
        public MonthPickerOptions Options { get; set; } = new MonthPickerOptions();

        public MonthItem? SelectedMonthItem { get; set; }
        private int ControlMonth { get; set; } = DateTime.Now.Month;
        public ObservableCollection<MonthList> DataSource { get; set; } = new ObservableCollection<MonthList>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await FillDataSource();

            if (CurrentMonthAsDefaultValue)
            {
                await SetCurrentMonth();
            }

        }

        protected async Task SetBaseMonth(int offset)
        {
            ControlMonth += offset;

            if (ControlMonth > MaximumMonth || ControlMonth < MinimalMonth)
            {
                ControlMonth -= offset;
                return;
            }

            await FillDataSource();
        }

        public async Task SetCurrentMonth()
        {
            if (ControlMonth != DateTime.Now.Month)
            {
                ControlMonth = DateTime.Now.Month;
                await FillDataSource();
            }

            await SetMonth(new MonthItem
            {
                Value = ControlMonth
            });
        }

        public async Task Clear()
        {
            if (SelectedMonthItem != null)
            {
                SelectedMonthItem.IsSelected = false;
                SelectedMonthItem = null;
            }

            CurrentValueAsString = null;

            await Task.CompletedTask;
        }

        public async Task SetMonth(MonthItem item)
        {
            if (item.ShowEmpty)
            {
                return;
            }

            foreach (MonthList list in DataSource)
            {
                foreach (MonthItem Month in list.Items)
                {
                    Month.IsSelected = item.Value == Month.Value;

                    if (Month.IsSelected)
                    {
                        SelectedMonthItem = Month;
                    }
                }
            }

            CurrentValueAsString = item.Value.ToString();

            StateHasChanged();

            await Task.CompletedTask;
        }

        public async Task FillDataSource()
        {
            int start = ControlMonth - 6;
            int end = ControlMonth + 5;
            int index = 0;
            DataSource.Clear();

            var item = new MonthList();
            DataSource.Add(item);

            for (int i = start; i <= end; i++)
            {
                if (index != 0 && index % 4 == 0)
                {
                    item = new MonthList();
                    DataSource.Add(item);
                }

                item.Items.Add(new MonthItem
                {
                    Value = i,
                    IsSelected = false,
                    ShowEmpty = i < MinimalMonth || i > MaximumMonth,
                });

                index++;
            }

            await Task.CompletedTask;
        }



        public class MonthList
        {
            public ObservableCollection<MonthItem> Items { get; set; } = new ObservableCollection<MonthItem>();
        }

        public class MonthItem
        {
            public int Value { get; set; }
            public bool IsSelected { get; set; }
            public bool ShowEmpty { get; set; }
        }

        public class MonthPickerOptions
        {
            public string CurrentMonthAlias = "Current Month";
            public string ClearAlias = "Clear";

        }
    }
}