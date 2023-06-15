namespace NDK.Core.Models
{
    public class NdkRegisterUser : NdkUser
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
