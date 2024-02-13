namespace NDK.Core.Models
{
    public class NdkListResponse<T> : NdkResponse<List<T>>
    {
        public long TotalItems  { get; set; }
        public long TotalPages { get; set; }
    }
}
