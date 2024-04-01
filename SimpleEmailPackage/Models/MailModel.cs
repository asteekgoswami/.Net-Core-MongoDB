using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMail.Models
{
    internal class MailModel
    {
        public string Sender { get; set; }
        public string To { get; set; }

        public List<string> CC { get; set; }
        public string? Subject { get; set; }

        public string Body { get; set; } = "Empty Mail";

    }
}
