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

        public int IdEssai { get; set; } // TODO: Le mettre à string une fois la mise à jour sur la BDD changer de descriptif_essai à titre_essai

        public string MailPropietaireEssai { get; set; }

        public string DateCreationEssai { get; set; }

        public bool EquipementSousPcVue { get; set; } // déterminer si on affiche ou pas le lien pour accèder aux équipements sous supervision
    }
}
