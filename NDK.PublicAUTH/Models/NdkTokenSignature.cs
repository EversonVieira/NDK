namespace NDK.PublicAuth.Models
{
    public class NdkTokenSignature
    {
        public string? SignedBy { get; set; }
        public string? TwoWaySignature { get; set; }
        public DateTime? SignedAt { get; set; }
    }

}
