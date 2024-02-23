namespace NDK.Auth.Core.Models
{
    public class NDKTokenSignature
    {
        public string? SignedBy { get; set; }
        public string? TwoWaySignature { get; set; }
        public DateTime? SignedAt { get; set; }
    }

}
