using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.RecupData
{
    public class InfosResasAdm
    {
        public string NomProjet { get; set; }

        public string NumProjet { get; set; }

        public int IdEssai { get; set; } 

        public string TitreEssai { get; set; }

        public string MailPropietaireEssai { get; set; }

        public string DateCreationEssai { get; set; }

        public bool EquipementSousPcVue { get; set; } // déterminer si on affiche ou pas le lien pour accèder aux équipements sous supervision
    }
}
