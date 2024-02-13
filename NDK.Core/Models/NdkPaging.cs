namespace NDK.Core.Models
{
    public class NdkPaging
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
    }

    public class NdkOrderItem
    {
        public string? Column { get; set; }

        public NdkOrderType OrderType { get; set; } = NdkOrderType.ASC;
    }
}
