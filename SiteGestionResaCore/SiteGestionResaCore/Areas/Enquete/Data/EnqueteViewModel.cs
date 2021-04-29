using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data
{
    public class EnqueteViewModel
    {

        public int IdEnquete { get; set; }
        public string TitreEssai { get; set; }
        public int IdEssai { get; set; }
        public string NumProjet { get; set; }
        public string TitreProj { get; set; }

        #region Méthodes

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Logiciel de réservation: ")]
        public int LogicielMeth { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Accesibilité/Clarté des procédures (équipements, nettoyage/désinfection, accès à la PFL,...: ")]
        public int AccessibiliteMeth { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentMéthodes { get; set; }
        #endregion

        #region Matériels (outils de travail)

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Disponibilité: ")]
        public int DisponibiliteMat { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Opérationnabilité: ")]
        public int OperationnabiliteMat { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Maintenance: ")]
        public int MaintenanceMat { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentMateriels { get; set; }
        #endregion

        #region Main d'oeuvre

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Disponibilité: ")]
        public int DisponibiliteMain { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Compétence: ")]
        public int CompetenceMain { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Formation/Tutorat: ")]
        public int FormationMain { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la Formation/Tutorat! ")]
        public bool IsConcerneFormationMain { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Reactivité: ")]
        public int ReactiviteMain { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMain { get; set; }
        #endregion

        #region Milieu

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Hygiène/Securité/Ergonomie/Réglementation: ")]
        public int HygieneMil { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Confidentialité: ")]
        public int ConfidentMil { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Espace de travail: ")]
        public int EspaceMil { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMil { get; set; }
        #endregion

        #region Matière

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Approvisionnement (lait produits laitières et produits de nettoyage): ")]
        public int ApprovisioMatiere { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Energie et fluides: ")]
        public int EnergieMatiere { get; set; }

        [Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Petit matériel et consommable (essuis main, charlotte, surchaussures,...: ")]
        public int MaterielMatiere { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMatiere { get; set; }
        #endregion

    }
}
