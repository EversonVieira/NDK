namespace NDK.Core.Models
{
    public class NDKPaging
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
    }

    public class NDKOrderItem
    {
        public string? Column { get; set; }

        public NDKOrderType OrderType { get; set; } = NDKOrderType.ASC;
    }
}
