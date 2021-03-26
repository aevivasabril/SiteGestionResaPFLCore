using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.DonneesUser
{
    public class InfosResa
    {
        public string NomProjet { get; set; }

        public string NumProjet { get; set; }

        public int IdEssai { get; set; }

        public string TitreEssai { get; set; }

        public bool EquipementSousPcVue { get; set; } // déterminer s'on affiche ou pas le lien pour accèder aux équipements sous supervision

        public DateTime DateCreationEssai { get; set; }

    }
}
