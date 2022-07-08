using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class evenement
    {
        public int id { get; set; }
        public DateTime date_creation { get; set; }
        public string message { get; set; }
    }
}
