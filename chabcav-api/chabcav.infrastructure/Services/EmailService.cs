using chabcav.infrastructure.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string emailAddress, string body)
        {
            var fromAddress = new MailAddress("donjon31395@gmail.com", "ChabCav Admin");
            var toAddress = new MailAddress(emailAddress);
            const string fromPassword = "krpl hsff qcgs akyp";
            const string subject = "Reset Password Request";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body, 
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}
