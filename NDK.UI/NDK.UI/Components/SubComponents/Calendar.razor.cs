namespace NDK.UI.Components.SubComponents
{
    public partial class Calendar
    {

        public Model CalendarOptions { get; set; } = new Model();



        public class Model
        {
            public int Year { get; set; }
            public int Month { get; set; }

            public string SundayAlias { get; set; } = "S";
            public string MondayAlias { get; set; } = "M";
            public string TuesdayAlias { get; set; } = "T";
            public string WednesdayAlias { get; set; } = "W";
            public string ThursdayAlias { get; set; } = "T";
            public string SaturdayAlias { get; set; } = "S";

            public List<int> AvailableYears = new List<int>();
            public List<int> AvailableMonths = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            public Model()
            {
                for(int i = 1800; i<= 9999; i++)
                {
                    AvailableYears.Add(i);
                }

                Year = DateTime.UtcNow.Year;
                Month = DateTime.UtcNow.Month;
            }

            
        }
    }
}