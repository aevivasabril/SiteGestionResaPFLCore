using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class rapport_metrologie
    {
        public int id { get; set; }
        public byte[] contenu_rapport { get; set; }
        public string nom_document { get; set; }
        public int capteurID { get; set; }
        public DateTime date_verif_metrologie { get; set; }
        public string type_rapport_metrologie { get; set; }

        public virtual capteur capteur { get; set; }
    }
}
