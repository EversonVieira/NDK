namespace NDK.Core.Models
{
    public class NDKListResponse<T> : NDKResponse<List<T>>
    {
        public long TotalItems  { get; set; }
        public long TotalPages { get; set; }

    }
}
