using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class capteur
    {
        public int id { get; set; }
        public string nom_capteur { get; set; }
        public string code_capteur { get; set; }
        public int equipementID { get; set; }
        public DateTime? date_prochaine_verif { get; set; }
        public DateTime? date_derniere_verif { get; set; }
        public double periodicite_metrologie { get; set; }
        public bool capteur_conforme { get; set; }
        public double emt_capteur { get; set; }
        public double? facteur_correctif { get; set; }

        public virtual equipement equipement { get; set; }
    }
}
