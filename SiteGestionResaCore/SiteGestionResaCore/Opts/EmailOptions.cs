using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Opts
{
    public class EmailOptions
    {
        public string From { get; set; }
        public string SMTP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
