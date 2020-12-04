using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public class ResEssaiChildViewModel
    {
        #region Champs "Essai" pour modification

        #region Liste des personnes en charge de la manip

        public int IdEssai { get; set; }
        /// <summary>
        /// Confidentialité du projet (restreint, ouvert ou confidentiel)
        /// </summary>
        [Required(ErrorMessage = "Champ 'Confidentialité' requis")]
        [Display(Name = "Confidentialité: *")]
        public string ConfidentialiteEssai { get; set; }

        /// <summary>
        /// Id d'un item de la liste provenance manipulateur essai
        /// </summary>
        [Required]
        [Display(Name = "Personne en charge des manips*")]
        [Range(1, 100, ErrorMessage = "Selectionnez un manipulateur pour l'essai")]
        public int SelectedManipulateurID { get; set; }

        public IEnumerable<SelectListItem> ManipProjItem { get; set; }

        #endregion

        #region Liste des produits entrée

        /// <summary>
        /// Id d'un item de la liste type produit d'entrée essai
        /// </summary>
        [Display(Name = "Type de produit d'entrée")]
        public int SelectedProductId { get; set; }

        public IEnumerable<SelectListItem> ProductItem { get; set; }

        #endregion
        /// <summary>
        /// String pour description sur le produit d'entrée
        /// </summary>
        [Display(Name = "Précision sur le produit: ")]
        public string PrecisionProduitIn { get; set; }

        /// <summary>
        /// string Quantité de produit
        /// </summary>
        [Display(Name = "Quantité de produit (Kg, L):")]
        public string QuantiteProduit { get; set; }

        #region Liste des provenances produits

        /// <summary>
        /// Id d'un item de la liste provenance du produit d'entrée essai
        /// </summary>
        [Display(Name = "Provenance produit")]
        public int SelectedProveProduitId { get; set; }

        public IEnumerable<SelectListItem> ProvenanceProduitItem { get; set; }


        #endregion

        #region Liste destination produits

        /// <summary>
        /// Id d'un item de la liste destinaison produit sortie essai
        /// </summary>
        [Display(Name = "Destination produits ")]
        public int SelectedDestProduit { get; set; }

        public IEnumerable<SelectListItem> DestProduitItem { get; set; }

        #endregion

        /// <summary>
        /// string à true ou false pour indiquer si le transport depend du STLO ou Autre
        /// </summary>
        [Required(ErrorMessage = "Champ 'Transport' requis")]
        [Display(Name = "Transport assuré par*: ")]
        public string TransportSTLO { get; set; }

        /// <summary>
        /// String pour rajouter un commentaire dans le formulaire
        /// </summary>
        [Display(Name = "Commentaire Essai")]
        public string CommentaireEssai { get; set; }

        public bool IsEssaiModifiable { get; set; }
        #endregion

        #region Infos "Essai" uniquement lecture


        private InfosEssai infosEssai = new InfosEssai();

        public InfosEssai InfosEssai
        {
            get { return infosEssai; }
            set { infosEssai = value; }
        }

        //public InfosEssai infosEssai { get; set; }

        #endregion
    }
}
