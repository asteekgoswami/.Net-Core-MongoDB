using Microsoft.Extensions.Options;
using MimeKit;
using mongodb.Models;
using mongodb.Repository.Interface;
using System.Net.Mail;
using MailKit.Net.Smtp;

namespace mongodb.Repository.Implementation
{
    public class EmailImplementation : IEmail
    {
        private readonly EmailSettings emailSettings;
        public EmailImplementation(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;   
        }

        public async Task<int> SendEmailAsync(MailRequest mailrequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(emailSettings.Email);
                email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
                email.Subject = mailrequest.Subject;
                /*var builder = new BodyBuilder();
                builder.TextBody = mailrequest.Body;
                email.Body = builder.ToMessageBody();*/

                email.Body = GetHtmlContent(mailrequest.Body);

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSettings.Email, emailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }

        private TextPart GetHtmlContent(string text)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Email Content</title>
        </head>
        <body>
            <div style='font-family: Arial, sans-serif;'>
                <h1>{text}</h1>
                <p>This is a sample email content with HTML formatting.</p>
                
                <img src='https://media.licdn.com/dms/image/D5603AQH4RC1pokhFOw/profile-displayphoto-shrink_800_800/0/1705266524667?e=2147483647&v=beta&t=IVD4VFsM7AwA-IJ17SH_tvp9Ya4pV9gZTuSmWcBivYc' alt='Sample Image' style='max-width: 100%; height: auto;' />
                
                <p>Additional information goes here.</p>
                
                <table style='width:100%; border-collapse: collapse; margin-top: 20px;'>
                    <tr style='background-color: #f2f2f2;'>
                        <th style='padding: 10px; border: 1px solid #ddd;'>Column 1</th>
                        <th style='padding: 10px; border: 1px solid #ddd;'>Column 2</th>
                    </tr>
                    <tr>
                        <td style='padding: 10px; border: 1px solid #ddd;'>Data 1</td>
                        <td style='padding: 10px; border: 1px solid #ddd;'>Data 2</td>
                    </tr>
                    <!-- Add more rows if needed -->
                </table>
            </div>
        </body>
        </html>
    ";

            return (TextPart)builder.ToMessageBody();
        }


    }
}
