using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace NDK.UI.Components
{
    public partial class NDKMonthPicker : NDKBaseInput<int>
    {
        [Parameter]
        public int MaximumMonth { get; set; }

        [Parameter]
        public int MinimalMonth { get; set; }

        [Parameter]
        public bool CurrentMonthAsDefaultValue { get; set; }

        [Parameter]
        public MonthPickerOptions Options { get; set; } = new MonthPickerOptions();

        public MonthItem? SelectedMonthItem { get; set; }

        private int ControlMonth { get; set; } = DateTime.Now.Month;

        public ObservableCollection<MonthList> DataSource { get; set; } = new ObservableCollection<MonthList>();

        [CascadingParameter]
        protected NDKCalendar? Calendar { get; set; }

        protected Dictionary<int, string> MonthNames { get; set; } = new Dictionary<int, string>();

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
            int start = 1;
            int end = 12;
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
                });

                index++;
            }

            if (MonthNames.Count == 0  || (MonthNames.ContainsKey(1) && !MonthNames[1].Equals(Options.JanAlias)))
            {
                this.MonthNames.Clear();

                this.MonthNames.Add(1, Options.JanAlias);
                this.MonthNames.Add(2, Options.FebAlias);
                this.MonthNames.Add(3, Options.MarAlias);
                this.MonthNames.Add(4, Options.AprilAlias);
                this.MonthNames.Add(5, Options.MayAlias);
                this.MonthNames.Add(6, Options.JuneAlias);
                this.MonthNames.Add(7, Options.JulyAlias);
                this.MonthNames.Add(8, Options.AugAlias);
                this.MonthNames.Add(9, Options.SepAlias);
                this.MonthNames.Add(10, Options.OctAlias);
                this.MonthNames.Add(11, Options.NovAlias);
                this.MonthNames.Add(12, Options.DecAlias);
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
        }

        public class MonthPickerOptions
        {
            public string CurrentMonthAlias { get; set; } = "Current Month";
            public string ClearAlias { get; set; } = "Clear";

            public string JanAlias { get; set; } = "Jan";
            public string FebAlias { get; set; } = "Feb";
            public string MarAlias { get; set; } = "Mar";
            public string AprilAlias { get; set; } = "April";
            public string MayAlias { get; set; } = "May";
            public string JuneAlias { get; set; } = "June";
            public string JulyAlias { get; set; } = "July";
            public string AugAlias { get; set; } = "Aug";
            public string SepAlias { get; set; } = "Sep";
            public string OctAlias { get; set; } = "Oct";
            public string NovAlias { get; set; } = "Nov";
            public string DecAlias { get; set; } = "Dec";

        }
    }
}