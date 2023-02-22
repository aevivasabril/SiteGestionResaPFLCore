using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class capteur
    {
        public capteur()
        {
            rapport_metrologie = new HashSet<rapport_metrologie>();
        }

        public int id { get; set; }
        public string? nom_capteur { get; set; }
        public string code_capteur { get; set; }
        public int equipementID { get; set; }
        public DateTime? date_prochaine_verif_int { get; set; }
        public DateTime? date_derniere_verif_int { get; set; }
        public double periodicite_metrologie_int { get; set; }
        public DateTime? date_prochaine_verif_ext { get; set; }
        public DateTime? date_derniere_verif_ext { get; set; }
        public double periodicite_metrologie_ext { get; set; }
        public bool capteur_conforme { get; set; }
        public double emt_capteur { get; set; }
        public double? facteur_correctif { get; set; }
        public string unite_mesure { get; set; }
        public string commentaire { get; set; }

        public virtual equipement equipement { get; set; }

        public virtual ICollection<rapport_metrologie> rapport_metrologie { get; set; }
    }
}
