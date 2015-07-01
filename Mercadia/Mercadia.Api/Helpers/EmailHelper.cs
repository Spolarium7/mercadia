using Mvc.Mailer;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Mercadia.Api.Helpers
{
    public static class EmailHelpers
    {
        /// <summary>
        /// SendGrid helper
        /// </summary>
        /// <param name="mvcMailMessage"></param>
        public static void SendNow(this MvcMailMessage mvcMailMessage)
        {
            var fromAddress = new MailAddress("noreply.mercadia@gmail.com", "Mercadia");
            const string fromPassword = "s3ct^mS3mpr@";
            const string subject = "Mercadia Email";
            string body = mvcMailMessage.Body;
            

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            foreach (var mailTo in mvcMailMessage.To)
            {
                var toAddress = new MailAddress(mailTo.Address, mailTo.DisplayName);

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }

        }

    }


}

