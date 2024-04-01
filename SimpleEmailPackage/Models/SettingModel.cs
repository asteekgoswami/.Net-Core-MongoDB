using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmail.Models
{
    internal class SettingModel
    {
        public string? host { get; set; } = "smtp.gmail.com";

        public int? port { get; set; } = 587;

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }
    }
}
