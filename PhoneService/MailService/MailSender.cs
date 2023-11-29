using System.Net.Mail;
using System.Net;

namespace PhoneService.MailService
{
    public class MailSender
    {
        public static void SendMail(string subject, string body, string eposta)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("blogproje114@gmail.com", "xmjxxzisaemudzcm");

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(eposta),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(eposta);

                client.Send(mailMessage);
            }

        }

    }
}

