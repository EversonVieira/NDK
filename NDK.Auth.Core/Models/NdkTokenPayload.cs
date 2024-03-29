﻿namespace NDK.Auth.Core.Models
{
    public class NDKTokenPayload
    {

        public string? UserId { get; set; }
        public string? IPAddress { get; set; }
        public string? MACAddress { get; set; }
        public List<NDKTokenRole> Roles { get; set; } = new List<NDKTokenRole>();
        public List<NDKTokenRolePermission> Permissions { get; set; } = new List<NDKTokenRolePermission>();

    }

}
