using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class enquete
    {
        public int id { get; set; }
        public DateTime date_envoi_enquete { get; set; }
        public int essaiId { get; set; }
        public bool? reponduEnquete { get; set; }
        public string fichierReponse { get; set; }
        public DateTime? date_reponse { get; set; }

        public virtual essai essai { get; set; }

    }
}
