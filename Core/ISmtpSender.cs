namespace Core
{
    public interface ISmtpSender
    {
        void SendEmail(string email, string subject, string text);
    }
}
