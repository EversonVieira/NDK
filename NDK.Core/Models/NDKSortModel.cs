namespace NDK.Core.Models
{
    public class NDKSortModel
    {
        public string? Field { get; set; }

        public NDKSortType SortType { get; set; } = NDKSortType.ASC;
    }
}
