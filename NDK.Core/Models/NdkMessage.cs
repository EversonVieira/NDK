namespace NDK.Core.Models
{
    public class NDKMessage
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
        public NDKMessageType Type { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? PropertyKey { get; set; }
    }
}
