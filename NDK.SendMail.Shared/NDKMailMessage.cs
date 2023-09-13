using System.Net.Mail;

namespace NDK.SendMail.Shared
{
    public class NdkMailMessage:MailMessage
    {
        public bool IsSent { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
