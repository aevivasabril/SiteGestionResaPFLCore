using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Evenements.Data
{
    public class EvenementsViewModel
    {
        public List<evenement> ListEvenements { get; set; }
        public int IdMessageXSupp { get; set; }

        [Display(Name = "Message à rajouter")]
        public string MessageXAjout { get; set; }
    }
}
