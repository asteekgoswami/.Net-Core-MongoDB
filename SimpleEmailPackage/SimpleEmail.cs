using MailKit.Security;
using MimeKit;
using SimpleEmail.Models;
using SimpleMail.Models;

namespace SimpleEmail

{
    public class Email
    {
        //porperties 
        public string Sender { get; set; } = "asteekgoswami650@gmail.com";
        public string To { get; set; } = "aasteekgoswami@gmail.com";

        public List<string> CC { get; set; }= new List<string>();

        public string? Subject { get; set; } = "Subject";

        public string Body { get; set; } = "Empty Mail";

        private string? host { get; set; } = "smtp.gmail.com";

        private int port { get; set; } = 587;

        private string UserEmail { get; set; }

        private string UserPassword { get; set; }


        /// <summary>
        /// Optional Functional if not call then automatically host = smtp.gmail.com and port=587 else pass arguements Setting(host,port)
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Setting(string? host, int port)
        {
            this.host = host;
            this.port = port;
        }

        /// <summary>
        /// Enter the (sender Email , App Password)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void Credetinals(string email, string password)
        {
            this.UserEmail = email;
            this.UserPassword = password;
        }
        /// <summary>
        /// Sending the Email
        /// </summary>
        public async void SendAsync()
        {
            MailModel mailModel = new MailModel()
            {
                Sender = this.Sender,
                To = this.To,
                CC= this.CC,
                Subject = this.Subject,
                Body = this.Body,
            };

            SettingModel settingModel = new SettingModel()
            {
                UserEmail = this.UserEmail,
                UserPassword = this.UserPassword,
                host = this.host,
                port = this.port,
            };

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailModel.Sender);
            email.To.Add(MailboxAddress.Parse(mailModel.To));
            foreach(var cc in mailModel.CC)
            {
                email.Cc.Add(MailboxAddress.Parse(cc)); ;
            }
            email.Subject = mailModel.Subject;

            //design a body
            var builder = new BodyBuilder();
            builder.HtmlBody = GenerateEmailBody(mailModel.Subject,mailModel.Body);
            email.Body = builder.ToMessageBody();

            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(settingModel.host, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(settingModel.UserEmail, settingModel.UserPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);


        }

        private string GenerateEmailBody(string subject , string emailMessage)
        {
            var body = $@"

<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Template</title>
    <style>
        body {{
            margin: 0;
            padding: 0;
            font-family: 'Arial', sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }}

        .email-container {{
            position: relative;
            max-width: 600px;
            width: 100%;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}

        .card-shape {{
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: linear-gradient(45deg, #3498db, #2c3e50);
            border-radius: 10px;
            z-index: -1;
        }}

        .header-box {{
            background-color: #3498db;
            color: #ffffff;
            text-align: center;
            padding: 20px;
            border-top-left-radius: 10px;
            border-top-right-radius: 10px;
        }}

        .organization-name {{
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 10px;
        }}

        .message-section {{
            padding: 20px;
            background-color: #f4f4f4;
            color: #2c3e50;
            border-bottom-left-radius: 10px;
            border-bottom-right-radius: 10px;
        }}

        .meta-info {{
            background-color: #2c3e50;
            color: #ecf0f1;
            text-align: center;
            padding: 10px;
        }}

        .copyright {{
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <!-- Email Container -->
    <div class=""email-container"">
        <div class=""card-shape""></div>

        <!-- Header Box -->
        <div class=""header-box"">
            <div class=""organization-name"">{subject}</div>
        </div>

        <!-- Message Section -->
        <div class=""message-section"">
            <p>
                {emailMessage}
            </p>
        </div>

        <!-- Meta Info -->
        <div class=""meta-info"">
            <p class=""copyright"">&copy; {2024}. Package By Asteek Goswami.</p>
        </div>
    </div>
</body>
</html>
";

            return body;
        }

    }
}
