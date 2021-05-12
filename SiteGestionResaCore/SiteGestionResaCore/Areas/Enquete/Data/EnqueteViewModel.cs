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

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Logiciel de réservation: ")]
        public int LogicielMeth { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Accesibilité/Clarté des procédures (équipements, nettoyage/désinfection, accès à la PFL,...: ")]
        public int AccessibiliteMeth { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentMéthodes { get; set; }
        #endregion

        #region Matériels (outils de travail)

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Disponibilité: ")]
        public int DisponibiliteMat { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Opérationnabilité: ")]
        public int OperationnabiliteMat { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Maintenance: ")]
        public int MaintenanceMat { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentMateriels { get; set; }
        #endregion

        #region Main d'oeuvre

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Disponibilité: ")]
        public int DisponibiliteMain { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Compétence: ")]
        public int CompetenceMain { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Formation/Tutorat: ")]
        public int FormationMain { get; set; }

        //[Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la Formation/Tutorat! ")]
        public bool IsNotConcerneFormationMain { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Reactivité: ")]
        public int ReactiviteMain { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMain { get; set; }
        #endregion

        #region Milieu

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Hygiène/Securité/Ergonomie/Réglementation: ")]
        public int HygieneMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Confidentialité: ")]
        public int ConfidentMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Espace de travail: ")]
        public int EspaceMil { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMil { get; set; }
        #endregion

        #region Matière

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Approvisionnement (lait produits laitières et produits de nettoyage): ")]
        public int ApprovisioMatiere { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Energie et fluides: ")]
        public int EnergieMatiere { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Petit matériel et consommable (essuis main, charlotte, surchaussures,...: ")]
        public int MaterielMatiere { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMatiere { get; set; }
        #endregion

    }
}
