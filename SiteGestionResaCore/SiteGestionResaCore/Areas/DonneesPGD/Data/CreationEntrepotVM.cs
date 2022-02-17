using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class CreationEntrepotVM
    {
        public int idEssai { get; set; }
        public List<ReservationsXEssai> ListReservationsXEssai { get; set; }
        public List<type_document> ListeTypeDoc { get; set; }
    }
}
