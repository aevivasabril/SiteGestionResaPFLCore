using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data
{
    public class MyReservationsViewModel
    {
        public List<InfosResasUser> ResasUser { get; set; }

        //public ResEssaiChildViewModel ChildVmModifEssai { get; set; } // vue partielle _ModifEssai

        public bool IsEssaiModifiable { get; set; } // déterminer quelle vue partielle on affiche

        #region Infos "Essai" uniquement lecture

        private ConsultInfosEssaiChildVM _consultInfosEssai = new ConsultInfosEssaiChildVM(); // Vue partielle _DisplayInfosEssai

        public ConsultInfosEssaiChildVM ConsultInfosEssai
        {
            get { return _consultInfosEssai; }
            set { _consultInfosEssai = value; }
        }

        #endregion

        #region Champs "Essai" pour modification

        #region Liste des personnes en charge de la manip

        public int IdEss { get; set; }
        /// <summary>
        /// Confidentialité du projet (restreint, ouvert ou confidentiel)
        /// </summary>
        [Display(Name = "Confidentialité: *")]
        public string ConfidentialiteEss { get; set; }

        /// <summary>
        /// Id d'un item de la liste provenance manipulateur essai
        /// </summary>
        [Display(Name = "Personne en charge des manips*")]
        [Range(1, 100, ErrorMessage = "Selectionnez un manipulateur pour l'essai")]
        public int SelecManipulateurID { get; set; }

        public IEnumerable<SelectListItem> ManiProjItem { get; set; }

        #endregion

        #region Liste des produits entrée

        /// <summary>
        /// Id d'un item de la liste type produit d'entrée essai
        /// </summary>
        [Display(Name = "Type de produit d'entrée")]
        public int SelectProductId { get; set; }

        public IEnumerable<SelectListItem> ProdItem { get; set; }

        #endregion
        /// <summary>
        /// String pour description sur le produit d'entrée
        /// </summary>
        [Display(Name = "Précision sur le produit: ")]
        public string PrecisionProdIn { get; set; }

        /// <summary>
        /// string Quantité de produit
        /// </summary>
        [Display(Name = "Quantité de produit (Kg, L):")]
        public string QuantProduit { get; set; }

        #region Liste des provenances produits

        /// <summary>
        /// Id d'un item de la liste provenance du produit d'entrée essai
        /// </summary>
        [Display(Name = "Provenance produit")]
        public int SelectProvProduitId { get; set; }

        public IEnumerable<SelectListItem> ProvProduitItem { get; set; }


        #endregion

        #region Liste destination produits

        /// <summary>
        /// Id d'un item de la liste destinaison produit sortie essai
        /// </summary>
        [Display(Name = "Destination produits ")]
        public int SelectDestProduit { get; set; }

        public IEnumerable<SelectListItem> DestProdItem { get; set; }

        #endregion

        /// <summary>
        /// string à true ou false pour indiquer si le transport depend du STLO ou Autre
        /// </summary>
        [Required(ErrorMessage = "Champ 'Transport' requis")]
        [Display(Name = "Transport assuré par*: ")]
        public string TranspSTLO { get; set; }

        /// <summary>
        /// String pour rajouter un commentaire dans le formulaire
        /// </summary>
        [Display(Name = "Commentaire Essai")]
        public string CommentEssai { get; set; }

        #endregion

        private List<InfosResasEquipement> _equipementsReserves = new List<InfosResasEquipement>();

        public List<InfosResasEquipement> EquipementsReserves
        {
            get { return _equipementsReserves; }
            set { _equipementsReserves = value; }
        }

        //public List<InfosResasEquipement> EquipementsReserves { get; set; }
    }
}
