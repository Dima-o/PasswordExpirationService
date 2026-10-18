using Password.Configuration;
using Password.Interfaces;
using System;
using System.Net;
using System.Net.Mail;

namespace Password.Services
{
    internal class MailService : IMailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _login;
        private readonly string _password;


        public MailService()
        {
            _smtpServer =
                AppConfiguration.Configuration["Mail:SmtpServer"];


            _port =
                int.Parse(
                    AppConfiguration.Configuration["Mail:Port"]);


            _login =
                AppConfiguration.Configuration["Mail:Login"];


            _password =
                AppConfiguration.Configuration["Mail:Password"];
        }


        public void SendWarning(string email, int daysLeft)
        {
            SendMail(
                email,
                "Истекает срок действия пароля",
                $"Ваш пароль истекает через {daysLeft} дней.");
        }


        public void SendNewPassword(string email, string newPassword)
        {
            SendMail(
                email,
                "Новый пароль",
                $"Ваш новый пароль: {newPassword}");
        }


        private void SendMail(
            string email,
            string subject,
            string body)
        {
            using (var client = new SmtpClient(
                _smtpServer,
                _port))
            {
                client.EnableSsl = true;


                client.Credentials =
                    new NetworkCredential(
                        _login,
                        _password);


                using (var message =
                    new MailMessage())
                {
                    message.From =
                        new MailAddress(_login);

                    message.To.Add(email);

                    message.Subject = subject;

                    message.Body = body;


                    client.Send(message);
                }
            }
        }
    }
}