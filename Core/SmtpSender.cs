using Serilog;
using System.Net;
using System.Net.Mail;

namespace Core
{
    public class SmtpSender : ISmtpSender
    {
        private readonly MailSettings Settings;
        private readonly ILogger Logger;
        private readonly MailAddress From;
        private readonly SmtpClient Smtp;

        public SmtpSender(ILogger logger, MailSettings mailSettings)
        {
            Logger = logger;
            Settings = mailSettings;
            Smtp = new SmtpClient(Settings.SmtpAddress, Settings.SmtpPort)
            {
                Credentials = new NetworkCredential(Settings.MailAddress, Settings.MailPassword)
            };
            From = new MailAddress(Settings.MailAddress, Settings.Domen);
            Smtp.EnableSsl = true;
            Smtp.UseDefaultCredentials = true;
        }
        public void SendEmail(string email, string subject, string text)
        {
            var to = new MailAddress(email);
            var message = new MailMessage(From, to)
            {
                Subject = subject,
                Body = text,
                IsBodyHtml = true
            };
            try
            {
                if (Settings.Enable)
                {
                    Smtp.Send(message);
                }
                Logger.Information($"Був відправиленний лист на адресу={email}.");
            }
            catch (Exception e)
            {
                Logger.Error($"Сервер не може відправити листа по адресі={email}");
                Logger.Error($"Виключення={e.Message}");
                Logger.Error($"Внутрішне виключення={e.InnerException?.Message}");
            }
        }
    }
}