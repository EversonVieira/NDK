using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using System.Collections.ObjectModel;

namespace NDK.UI.Components
{
    public partial class NDKYearPicker : NDKBaseInput<int>
    {
        [Parameter]
        public int MinimalYear { get; set; } = 1800;

        [Parameter]
        public int MaximumYear { get; set; } = 9999;

        [Parameter]
        public bool CurrentYearAsDefaultValue { get; set; }

        [Parameter]
        public YearPickerOptions Options { get; set; } = new YearPickerOptions();

        public YearItem? SelectedYearItem { get; set; }
        private int CurrentYear { get; set; } = DateTime.Now.Year;
        public ObservableCollection<YearList> DataSource { get; set; } = new ObservableCollection<YearList>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await FillDataSource();

            if (CurrentYearAsDefaultValue)
            {
                await SetCurrentYear();
            }

        }

        public async Task SetCurrentYear()
        {
            await SetYear(new YearItem
            {
                Value = CurrentYear
            });
        }

        public async Task Clear()
        {
            if (SelectedYearItem != null)
            {
                SelectedYearItem.IsSelected = false;
                SelectedYearItem = null;
            }

            CurrentValueAsString = null;

            await Task.CompletedTask;
        }

        public async Task SetYear(YearItem item)
        {
            if (item.ShowEmpty)
            {
                return;
            }

            foreach (YearList list in DataSource)
            {
                foreach (YearItem year in list.Items)
                {
                    year.IsSelected = item.Value == year.Value;

                    if (year.IsSelected)
                    {
                        SelectedYearItem = year;
                    }
                }
            }

            CurrentValueAsString = item.Value.ToString();

            StateHasChanged();

            await Task.CompletedTask;
        }

        public async Task FillDataSource(int offset = 0)
        {
            CurrentYear += offset;

            int start = CurrentYear - 6;
            int end = CurrentYear + 5;
            int index = 0;
            DataSource.Clear();

            var item = new YearList();
            DataSource.Add(item);

            for (int i = start; i <= end; i++)
            {
                if (index != 0 && index % 4 == 0)
                {
                    item = new YearList();
                    DataSource.Add(item);
                }

                item.Items.Add(new YearItem
                {
                    Value = i,
                    IsSelected = false,
                    ShowEmpty = i < MinimalYear || i > MaximumYear,
                });

                index++;
            }

            await Task.CompletedTask;
        }



        public class YearList
        {
            public ObservableCollection<YearItem> Items { get; set; } = new ObservableCollection<YearItem>();
        }

        public class YearItem
        {
            public int Value { get; set; }
            public bool IsSelected { get; set; }
            public bool ShowEmpty { get; set; }
        }

        public class YearPickerOptions
        {
            public string CurrentYearAlias = "Current Year";
            public string ClearAlias = "Clear";

        }
    }
}