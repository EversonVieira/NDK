namespace NDK.Core.Models
{
    public class NdkListResponse<T> : NdkResponse<List<T>>
    {
        public long AvailableItems { get; set; }
    }
}
