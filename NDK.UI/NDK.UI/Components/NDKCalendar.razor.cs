using Microsoft.AspNetCore.Components;
using NDK.UI.Components.Common;
using System.Collections.ObjectModel;

namespace NDK.UI.Components
{
    public partial class NDKCalendar : NDKBaseInput<DateTime>
    {
        [Parameter]
        public string DateFormat { get; set; } = "yyyy/MM/dd hh:mm:ss";

        [Parameter]
        public CalendarOptions Options { get; set; } = new CalendarOptions();


        protected ObservableCollection<WeekList> DataSource = new ObservableCollection<WeekList>();

        private List<DayItem> AllDays = new List<DayItem>();
        private DayItem? SelectedDay;
        public NDKCalendar()
        {
            FiilDataSource();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }



        private async Task SetSelectedDateAsToday()
        {
            int day = DateTime.Now.Day;
            SelectedDay = AllDays.Find(x => x.Day == day);

            Options.Year = DateTime.UtcNow.Year;
            Options.Month = DateTime.UtcNow.Month;

            if (SelectedDay != null)
            {
                await OnDaySelect(SelectedDay);
            }
        }

        public async Task OnClear()
        {
            if (this.SelectedDay != null)
            {
                this.SelectedDay.IsSelected = false;
                this.SelectedDay = null;
            }

            this.CurrentValueAsString = null;

            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task OnDaySelect(DayItem day)
        {
            foreach (var wk in DataSource)
            {
                foreach (var item in wk.DayList)
                {
                    if (item.Day == day.Day)
                    {
                        if (!item.ShowEmpty)
                        {
                            item.IsSelected = true;
                            SelectedDay = item;
                        }
                    }
                    else
                    {
                        item.IsSelected = false;
                    }
                }
            }



            await UpdateValue();


            StateHasChanged();
        }


        public async Task UpdateValue(bool dataSourceUpdate = false)
        {

            if (dataSourceUpdate)
            {
                this.FiilDataSource();
                StateHasChanged();
            }

            if (SelectedDay == null)
            {
                CurrentValueAsString = null;
                return;
            }

            if (SelectedDay.ShowEmpty)
            {
                CurrentValueAsString = null;
                return;
            }

            CurrentValueAsString = new DateTime(Options.Year, Options.Month, SelectedDay.Day).ToString(DateFormat);

            await Task.CompletedTask;
        }

        protected override DateTime ParseValueFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var result = default(DateTime);
                return result;
            }

            string[] dateAndTime = value.Split();

            string[] dateParts = dateAndTime[0].Split('/');

            string[] hourParts = dateAndTime[1].Split(':');


            return new DateTime(year: Convert.ToInt32(dateParts[0]),
                                month: Convert.ToInt32(dateParts[1]),
                                day: Convert.ToInt32(dateParts[2]),
                                hour: Convert.ToInt32(hourParts[0]),
                                minute: Convert.ToInt32(hourParts[1]),
                                second: Convert.ToInt32(hourParts[2]));
;        }


        protected string GetMonthLabel(int month)
        {
            if (month <= 0 || month > 12)
            {
                return string.Empty;
            }

            return Options.MonthLabels[month];
        }
        private void FiilDataSource()
        {
            int AvailableDays = DateTime.DaysInMonth(Options.Year, Options.Month);
            int weekIndex = 1;

            ObservableCollection<WeekList> weekList = new ObservableCollection<WeekList>();

            WeekList item = new WeekList();

            weekList.Add(item);
            bool newWeek = false;

            for (int i = 1; i <= AvailableDays; i++)
            {
                DayOfWeek dayOfWeek = new DateTime(Options.Year, Options.Month, i).DayOfWeek;

                if (i == 1 && dayOfWeek > DayOfWeek.Sunday)
                {
                    for (int k = 0; k < (int)dayOfWeek; k++)
                    {
                        item.DayList.Add(new DayItem { ShowEmpty = true, DayOfWeek = (DayOfWeek)k });
                    }
                }

                if (newWeek)
                {
                    item = new WeekList()
                    {
                        WeekIndex = weekIndex,
                    };

                    weekList.Add(item);
                    newWeek = false;
                }


                item.DayList.Add(new DayItem { DayOfWeek = dayOfWeek, Day = i });

                if (dayOfWeek == DayOfWeek.Saturday)
                {
                    newWeek = true;
                    weekIndex++;
                }
            }

            foreach (var list in weekList)
            {
                list.DayList = new ObservableCollection<DayItem>(list.DayList.OrderBy(x => x.DayOfWeek).ToList());
            }

            weekList = new ObservableCollection<WeekList>(weekList.OrderBy(x => x.WeekIndex).ToList());

            foreach(var wi in weekList)
            {
                AllDays.AddRange(wi.DayList);
            }

            this.DataSource = weekList;
        }

        public async Task OnClickHandlerAsync()
        {
            await this.SetSelectedDateAsToday();

            await UpdateValue();

        }

        


        public class WeekList
        {
            public int WeekIndex;
            public ObservableCollection<DayItem> DayList { get; set; } = new ObservableCollection<DayItem>();
        }

        public class DayItem
        {
            public bool ShowEmpty;
            public DayOfWeek DayOfWeek { get; set; }
            public int Day { get; set; }
            public bool IsSelected { get; set; }
        }



        public class CalendarOptions
        {
            public int Year { get; set; } = DateTime.UtcNow.Year;
            public int Month { get; set; } = DateTime.UtcNow.Month;

            public string TodayAlias { get; set; } = "Today";
            public string Clear { get; set; } = "Clear";
            public Dictionary<int, string> MonthLabels { get; set; } = new Dictionary<int, string>();

            public string SundayAlias { get; set; } = "S";
            public string MondayAlias { get; set; } = "M";
            public string TuesdayAlias { get; set; } = "T";
            public string WednesdayAlias { get; set; } = "W";
            public string ThursdayAlias { get; set; } = "T";
            public string FridayAlias { get; set; } = "F";
            public string SaturdayAlias { get; set; } = "S";


            public List<int> AvailableYears = new List<int>();
            public List<int> AvailableMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };


            public CalendarOptions()
            {
                for (int i = 1800; i <= 9999; i++)
                {
                    AvailableYears.Add(i);
                }

                Year = DateTime.UtcNow.Year;
                Month = DateTime.UtcNow.Month;
                FillMonthLabels();
            }

            public virtual void FillMonthLabels()
            {
                MonthLabels.Add(1, "January");
                MonthLabels.Add(2, "February");
                MonthLabels.Add(3, "March");
                MonthLabels.Add(4, "April");
                MonthLabels.Add(5, "May");
                MonthLabels.Add(6, "Jun");
                MonthLabels.Add(7, "July");
                MonthLabels.Add(8, "August");
                MonthLabels.Add(9, "September");
                MonthLabels.Add(10, "October");
                MonthLabels.Add(11, "November");
                MonthLabels.Add(12, "December");

            }


            public DayOfWeek GetFirstDayOfTheWeek()
            {
                var Date = new DateTime(Year, Month, 1);

                return Date.DayOfWeek;

            }
        }
    }
}