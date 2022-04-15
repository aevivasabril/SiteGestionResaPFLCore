using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip
{
    public class EquipsXModifVM
    {
        public List<InfosEquipement> ListeEquipements { get; set; }
        public string NomZone { get; set; }

        #region Id's à retenir pour les actions
        public int IdZone { get; set; }
        public int IdEquipement { get; set; }
        public int IdDoc { get; set; }

        public int IdFicheMat { get; set; }
        public string NomEquipement { get; set; }
        #endregion

        [Required(ErrorMessage ="Numéro GMAO requis")]
        [RegularExpression(@"[A-Z]{1,}[0-9]{1,}", ErrorMessage = "Format numéro GMAO incorrect")]
        public string NumGmao { get; set; }
    }
}
