using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public class InfosReservations
    {
        public string NumProjet { get; set; }
        public string TitreProjet { get; set; }
        public string RespProjet { get; set; }
        public string TypeProjet { get; set; }
        public string TitreEssai { get; set; }
        public string NomEquipement { get; set; }
        public int IdEssai { get; set; }
        public string NomOrganisme { get; set; }
        public string NomEquipe { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateDebutResa { get; set; }
        public DateTime DateFinResa { get; set; }


    }
}
