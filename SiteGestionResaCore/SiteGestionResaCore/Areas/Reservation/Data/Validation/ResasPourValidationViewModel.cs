using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public class ResasPourValidationViewModel
    {
        public IList<InfosAffichage> resasAValider { get; set; }

        public ConsultInfosEssaiChildVM InfosEssai { get; set; }

        public InfosProjet InfosProj { get; set; }

        public List<InfosReservation> Reservations { get; set; }

        public List<InfosConflit> InfosConflits { get; set; }

        private int _idEss = 0;
        public int IdEss
        {
            get { return _idEss; }
            set { _idEss = value; }
        }

        private string _raisonRefus;
        [Required(ErrorMessage = "Champ 'Raison refus' requis")]
        public string RaisonRefus
        {
            get { return _raisonRefus; }
            set { _raisonRefus = value; }
        }

    }
}
