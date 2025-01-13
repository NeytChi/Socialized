namespace Core
{
    public class MailSettings
    {
        public required string Domen { get; set; }
        public required string MailAddress { get; set; }
        public required string MailPassword { get; set; }
        public required string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
        public bool Enable { get; set; }
    }
}
