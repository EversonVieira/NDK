namespace NDK.Auth.Core.Models
{
    public class NdkTokenRole
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public List<NdkTokenRolePermission> Permissions { get; set; } = new List<NdkTokenRolePermission>();
    }

}
