using NDK.UI.Components.Common;

namespace NDK.UI.Components.SubComponents
{
    public partial class Calendar : NDKBaseInput<DateTime>
    {

        public CalendarOptions Options { get; set; } = new CalendarOptions();

        public List<WeekList> DataSource = new List<WeekList>();

        public Calendar()
        {
            FiilDataSource();
        }

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

            List<WeekList> weekList = new List<WeekList>();

            WeekList item = new WeekList();

            weekList.Add(item);
            bool newWeek = false;

            for (int i = 1; i <= AvailableDays; i++)
            {
                DayOfWeek dayOfWeek = new DateTime(this.Options.Year, this.Options.Month, i).DayOfWeek;

                if ((i == 1) && dayOfWeek > DayOfWeek.Sunday)
                {
                    for (int k = 0; k <= (int)dayOfWeek; k++)
                    {
                        item.DayList.Add(new DayList { ShowEmpty = true, DayOfWeek = (DayOfWeek)k });
                    }
                }

                if (newWeek)
                {
                    item = new WeekList()
                    {
                        WeekIndex = i,
                    };

                    weekList.Add(item);

                    newWeek = false;
                }


                item.DayList.Add(new DayList { DayOfWeek = dayOfWeek, Day = i });

                if (dayOfWeek == DayOfWeek.Saturday)
                {
                    newWeek = true;
                    weekIndex++;
                }
            }

            foreach (var list in weekList)
            {
                list.DayList = list.DayList.OrderBy(x => x.DayOfWeek).ToList();
            }

            weekList = weekList.OrderBy(x => x.WeekIndex).ToList();

            DataSource = weekList;
        }



        public class WeekList
        {
            public int WeekIndex;
            public List<DayList> DayList { get; set; } = new List<DayList>();
        }

        public class DayList
        {
            public bool ShowEmpty;
            public DayOfWeek DayOfWeek { get; set; }
            public int Day { get; set; }
        }

        public class CalendarOptions
        {
            public int Year { get; set; } = DateTime.UtcNow.Year;
            public int Month { get; set; } = DateTime.UtcNow.Month;

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
                var Date = new DateTime(this.Year, this.Month, 1);

                return Date.DayOfWeek;

            }
        }
    }
}