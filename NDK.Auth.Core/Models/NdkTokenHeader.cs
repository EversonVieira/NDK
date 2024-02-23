namespace NDK.Auth.Core.Models
{
    public class NDKTokenHeader
    {
        public string? TokenType { get; set; }
        public string? TokenPublicKey { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

}
