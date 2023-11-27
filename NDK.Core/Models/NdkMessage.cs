﻿namespace NDK.Core.Models
{
    public class NdkMessage
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
        public NdkMessageType Type { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? PropertyKey { get; set; }
    }
}
