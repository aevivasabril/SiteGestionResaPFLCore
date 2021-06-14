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

        #region 1 Méthodes

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Logiciel de réservation: ")]
        [Display(Name = "Est-ce que le logiciel de réservation satisfait vos attentes (réservation des pilotes, récupération des données,...)?: ")]
        public int LogicielMeth { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Accesibilité/Clarté des procédures (équipements, nettoyage/désinfection, accès à la PFL,...: ")]
        [Display(Name = "Les procédures relatives aux modalités d'utilisation de la PFL, à l'utilisation des materiels, utilisation nettoyage étaient-elles claires?")]
        public int AccessibiliteMeth { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentMéthodes { get; set; }
        #endregion

        #region 2 Matériels (outils de travail)

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Disponibilité: ")]
        [Display(Name = "Est-ce que les matériels sont disponibles facilement?: ")]
        public int DisponibiliteMat { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Opérationnabilité: ")]
        [Display(Name = "Les matériels sont-ils en bon état de fonctionnement et de propreté?: ")]
        public int OperationnabiliteMat { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "L'assistance technique a-t-elle été satisfaisante?: ")]
        public int MaintenanceMat { get; set; }

        //[Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la rubrique")]
        public bool IsNotConcerneMaintMat { get; set; }
       

        [Display(Name = "Commentaire: ")]
        public string CommentMateriels { get; set; }
        #endregion

        #region 3 Main d'oeuvre

        /*[Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Disponibilité: ")]
        [Display(Name = "Le personnel PFL a été disponible pour vous informer et vous aider à la préparation et réalisation des essais?: ")]
        public int DisponibiliteMain { get; set; }

        // TODO: A SUPPRIMER
        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = " SUPPRIMER Compétence: ")]
        public int CompetenceMain { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Formation/Tutorat: ")]
        [Display(Name = "A SUPPRIMER Pour les nouveaux utilisateurs: Comment jugez vous la formation reçue?")]
        public int FormationMain { get; set; }

        //[Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la rubrique")]
        public bool IsNotConcerneFormationMain { get; set; }

        // TODO: A SUPPRIMER
        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "SUPPRIMER Le personnel PFL était-il reactif à des problèmes que vous avez pu rencontrer (ex: panne d'un équipement)? ")]
        public int ReactiviteMain { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMain { get; set; }*/

        #endregion

        #region 3 Milieu

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Hygiène/Securité/Ergonomie/Réglementation: ")]
        [Display(Name = "L'état de proprète était-il satisfaisant?: ")]
        public int HygieneMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Confidentialité: ")]
        [Display(Name = "La confidentialité de vos essais était-elle assurée?: ")]
        public int ConfidentMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Est-ce que votre environnement de travail était-il secure?: ")]
        public int SecureMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Le  materiel annexe aux appareils était-il disponible?: ")]
        public int MaterielMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Comment jugez vous le fonctionnement des énergies et fluides au cours de votre essai?: ")]
        public int EnergieMil { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMil { get; set; }

        #endregion

        #region 4 Matière
        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "L'approvisionnement de matière première était-il satisfaisant?: ")]
        public int ApprovisioMatiere { get; set; }

        //[Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la rubrique")]
        public bool IsNotConcerneApproviMat { get; set; }
        
        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Disponibilité des produits de nettoyage?: ")]
        public int NettoMatiere { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "L'approvisionnement des consommables(essuis main, surchaussures,...) était-il satisfaisant?: ")]
        public int MaterielMatiere { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMatiere { get; set; }
        #endregion

    }
}
