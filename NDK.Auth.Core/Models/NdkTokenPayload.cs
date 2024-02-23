namespace NDK.Auth.Core.Models
{
    public class NdkTokenPayload
    {
        public string? UserId { get; set; }

        public List<NdkTokenClaim> Claims { get; set; } = new List<NdkTokenClaim>();

        public List<NdkTokenRole> Roles { get; set; } = new List<NdkTokenRole>();
    }

}
