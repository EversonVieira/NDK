namespace NDK.Auth.Core.Models
{
    public class NDKTokenRole
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public List<NDKTokenRolePermission> Permissions { get; set; } = new List<NDKTokenRolePermission>();
    }

}
