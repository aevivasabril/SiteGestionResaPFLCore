using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public class ModifMaintenanceVM
    {
        public string NumMaintenance { get; set; }

        public string OpenModifInter { get; set; }

        public MaintenanceInfos InfosMaint { get; set; }

        public List<EquipCommunXInterv> ListEquipsCommuns { get; set; }

        public List<EquipPflXIntervention> ListeEquipsPfl { get; set; }

        /// <summary>
        /// Id intervention équipement adjacent
        /// </summary>
        public int IdIntervCom { get; set; }

        /// <summary>
        /// Id intervention équipement PFL
        /// </summary>
        public int IdIntervPfl { get; set; }

        /// <summary>
        /// Raison de la suppression
        /// </summary>
        public string RaisonSuppression { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Date Fin' est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Nouvelle date fin : ")]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date fin est requis")]
        [Display(Name = "Créneau fin")]
        public string DatePickerFin_Matin { get; set; }

        #region Ouvrir/ Fermer la vue suite à la recherche d'une opération

        public string Ouvert = "";
        public string Ferme = "none";

        #endregion
    }
}
