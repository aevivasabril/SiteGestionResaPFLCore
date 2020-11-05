using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    /// <summary>
    /// Class Serializable (ouverture d'une session pour stocker des données en local) utilisée lors de la réservation des équipements 
    /// </summary>
    [Serializable]
    public class FormulaireProjetViewModel
    {
        #region Copier un projet + essai

        #region Liste des essais correspondant au numéro projet

        /// <summary>
        /// boolean indiquant si un numéro de projet existe
        /// </summary>
        public bool ProjetValide { get; set; }

        private List<EssaiUtilisateur> _essaiUtilisateur = new List<EssaiUtilisateur>();
        /// <summary>
        /// Liste des essais + manipulateur
        /// </summary>
        public List<EssaiUtilisateur> EssaiUtilisateur
        {
            get { return _essaiUtilisateur; }
            set { _essaiUtilisateur = value; }
        }

        /// <summary>
        /// Id de l'essai selectionné à partir du dropdownlist 
        /// </summary>
        [Display(Name = "Liste des Essais")]
        [Range(1, 100, ErrorMessage = "Selectionnez un Essai")]
        public int SelectedEssaiId { get; set; }

        private IEnumerable<SelectListItem> _essaiItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> EssaiItem
        {
            get { return _essaiItem; }
            set { _essaiItem = value; }
        }


        // TODO: Essayer de mettre une date de réservation plutôt que la date de création à voir si possible
        /// <summary>
        /// Création d'une liste des item avec des détails d'un essai
        /// </summary>
        /*public IEnumerable<SelectListItem> EssaiItem
        {
            get
            {
                    
                var allOrgs = EssaiUtilisateur.Select(f => new SelectListItem
                {
                    Value = f.CopieEssai.id.ToString(),
                    Text = "Essai crée le " + f.CopieEssai.date_creation.ToString() + " - Manipulateur Essai: " + f.user.nom +", "+ f.user.prenom + " - Commentaire essai: " +
                           f.CopieEssai.commentaire + " - Type produit entrant: "+ f.CopieEssai.type_produit_entrant + " -" + f.CopieEssai.quantite_produit
                }); 
                return DefaultEssaiItem.Concat(allOrgs);
                
            }
        }

        /// <summary>
        /// Premier item par défaut de la dropdownlist copie d'essai
        /// </summary>
        public IEnumerable<SelectListItem> DefaultEssaiItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Sélectionnez un Essai -"
                }, count: 1);
            }
        }*/

        #endregion

        #endregion

        #region champs "Projet"

        /// <summary>
        /// Titre du projet
        /// </summary>
        [Required]
        [Display(Name = "Titre Projet*")]
        [RegularExpression(@"^[\w][^\/;\\.!:*?,]*$", ErrorMessage = "Le format est incorrect, evitez les caractères suivants:  /;\\.!:*?,")]
        public string TitreProjet { get; set; }

        // Numero du projet au format (12-345)
        [Required]
        [Display(Name = "Numéro de projet* (Ex format: 12-345)")] // il faut respecter le numéro 
        [RegularExpression(@"^[0-9]{2}[-][0-9]{3}$", ErrorMessage = "Le numéro de projet ne respecte pas le format de la base action")]
        public string NumProjet { get; set; }

        /// <summary>
        /// Description d'un projet 
        /// </summary>
        [Display(Name = "Description du projet")]
        public string DescriptionProjet { get; set; }

        #region Liste pour le type de projet

        private List<ld_type_projet> _listeTypeProjet = new List<ld_type_projet>();
        /// <summary>
        /// Liste obtenu à partir d'une table dans la base de données contenant les valeurs pour les types de projet
        /// </summary>
        public List<ld_type_projet> ListeTypeProjet
        {
            get { return _listeTypeProjet; }
            set { _listeTypeProjet = value; }
        }

        /// <summary>
        /// Id d'un item selectionné pour le type projet
        /// </summary>
        [Display(Name = "Type de projet")]
        public int SelectTypeProjetId { get; set; }

        private IEnumerable<SelectListItem> _typeProjetItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> TypeProjetItem
        {
            get { return _typeProjetItem; }
            set { _typeProjetItem = value; }
        }

        #endregion

        #region Liste pour l'origine financement

        private List<ld_financement> _listeFinancement;
        /// <summary>
        /// Liste obtenu à partir d'une table dans la base de données contenant les valeurs pour les types de financement
        /// </summary>
        public List<ld_financement> ListeFinancement
        {
            get { return _listeFinancement; }
            set { _listeFinancement = value; }
        }

        /// <summary>
        /// Id d'un item selectionnée pour le type de financement
        /// </summary>
        [Display(Name = "Origine Financement")]
        public int SelectFinancementId { get; set; }

        private IEnumerable<SelectListItem> _typeFinancItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> TypefinancementItem
        {
            get { return _typeFinancItem; }
            set { _typeFinancItem = value; }
        }

        #endregion

        #region Liste des organismes

        private List<organisme> _listeOrganismes;
        /// <summary>
        /// Liste obtenu à partir d'une table dans la base de données contenant les valeurs pour les noms des organismes STLO
        /// </summary>
        public List<organisme> ListeOrganismes
        {
            get { return _listeOrganismes; }
            set { _listeOrganismes = value; }
        }

        /// <summary>
        /// Id d'un item selectionné pour le organisme
        /// </summary>
        [Display(Name = "Nom de l'entreprise si privé")]
        public int SelectedOrganId { get; set; }

        private IEnumerable<SelectListItem> _organItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> OrganItem
        {
            get { return _organItem; }
            set { _organItem = value; }
        }

        #endregion

        #region Liste des utilisateurs pour selection du responsable projet (uniquement utilisateurs avec un compte valide)

        private List<utilisateur> _usersWithAccess;
        /// <summary>
        /// Liste des utilisateurs avec un compte valide
        /// </summary>
        public List<utilisateur> UsersWithAccess
        {
            get { return _usersWithAccess; }
            set { _usersWithAccess = value; }
        }

        /// <summary>
        /// Id d'un item de la liste des responsables projet
        /// </summary>
        [Required]
        [Display(Name = "Mail responsable projet*")]
        [Range(1, 100, ErrorMessage = "Selectionnez un responsable projet")]
        public int SelectedRespProjId { get; set; }

        private IEnumerable<SelectListItem> _respProjItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> RespProjItem
        {
            get { return _respProjItem; }
            set { _respProjItem = value; }
        }


        /// <summary>
        /// Création d'une liste des responsables projet (dropdownlist)
        /// </summary>
        /*public IEnumerable<SelectListItem> RespProjItem
        {
            get
            {
                var allOrgs = UsersWithAccess.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )"
                }); ;
                return DefaultRespItem.Concat(allOrgs);
            }
        }

        /// <summary>
        /// Entête de la liste pour selectionner un organisme
        /// </summary>
        public IEnumerable<SelectListItem> DefaultRespItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Sélectionnez un responsable -"
                }, count: 1);
            }
        }*/

        #endregion

        #region Liste des options provenance projet

        private List<ld_provenance> _listeProvenance;
        /// <summary>
        /// Liste des options pour provenance projet à partir de la base de données 
        /// </summary>
        public List<ld_provenance> ListeProvenance
        {
            get { return _listeProvenance; }
            set { _listeProvenance = value; }
        }

        /// <summary>
        /// Id d'un item de la liste provenance projet
        /// </summary>
        [Display(Name = "Provenance projet")]
        public int SelectedProvenanceId { get; set; }

        private IEnumerable<SelectListItem> _provenanceItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> ProvenanceItem
        {
            get { return _provenanceItem; }
            set { _provenanceItem = value; }
        }

        #endregion

        #endregion

        #region Champs "Essai"

        #region Liste des personnes en charge de la manip

        /// <summary>
        /// Confidentialité du projet (restreint, ouvert ou confidentiel)
        /// </summary>
        [Required]
        [Display(Name = "Confidentialité: *")]
        public string ConfidentialiteEssai { get; set; }

        /// <summary>
        /// Id d'un item de la liste provenance manipulateur essai
        /// </summary>
        [Required]
        [Display(Name = "Personne en charge des manips*")]
        [Range(1, 100, ErrorMessage = "Selectionnez un manipulateur pour l'essai")]
        public int SelectedManipulateurID { get; set; }

        private IEnumerable<SelectListItem> _manipProjItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> ManipProjItem
        {
            get { return _manipProjItem; }
            set { _manipProjItem = value; }
        }

        /// <summary>
        /// Création d'une liste utilisateurs "manipulateur" de l'essai
        /// </summary>
        /*public IEnumerable<SelectListItem> ManipProjItem
        {
            get
            {
                var allOrgs = UsersWithAccess.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )"
                }); ;
                return DefaultManipItem.Concat(allOrgs);
            }
        }

        /// <summary>
        /// Entete de la liste des utilisateurs "manipulateurs" pour un essai
        /// </summary>
        public IEnumerable<SelectListItem> DefaultManipItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Sélectionnez un Manipulateur -"
                }, count: 1);
            }
        }*/
        #endregion

        #region Liste des produits entrée

        private List<ld_produit_in> _listeProduitsIn;
        /// <summary>
        /// Liste des options pour le type produit en entrée à partir de la base de données 
        /// </summary>
        public List<ld_produit_in> ListeProduitsIn
        {
            get { return _listeProduitsIn; }
            set { _listeProduitsIn = value; }
        }

        /// <summary>
        /// Id d'un item de la liste type produit d'entrée essai
        /// </summary>
        [Display(Name = "Type de produit d'entrée")]
        public int SelectedProductId { get; set; }

        private IEnumerable<SelectListItem> _productItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> ProductItem
        {
            get { return _productItem; }
            set { _productItem = value; }
        }


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

        private List<ld_provenance_produit> _listeProvenanceProduit;
        /// <summary>
        /// Liste des options pour indiquer la provenance du produit en entrée à partir de la base de données 
        /// </summary>
        public List<ld_provenance_produit> ListeProvenanceProduit
        {
            get { return _listeProvenanceProduit; }
            set { _listeProvenanceProduit = value; }
        }

        /// <summary>
        /// Id d'un item de la liste provenance du produit d'entrée essai
        /// </summary>
        [Display(Name = "Provenance produit")]
        public int SelectedProveProduitId { get; set; }

        private IEnumerable<SelectListItem> _provenanceProduitItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> ProvenanceProduitItem
        {
            get { return _provenanceProduitItem; }
            set { _provenanceProduitItem = value; }
        }

        #endregion

        #region Liste destination produits

        private List<ld_destination> _listeDestProduit;
        /// <summary>
        /// Liste des options pour la destinaison des produits sortie à partir de la base de données
        /// </summary>
        public List<ld_destination> ListeDestProduit
        {
            get { return _listeDestProduit; }
            set { _listeDestProduit = value; }
        }

        /// <summary>
        /// Id d'un item de la liste destinaison produit sortie essai
        /// </summary>
        [Display(Name = "Destination produits ")]
        public int SelectedDestProduit { get; set; }

        private IEnumerable<SelectListItem> _destProduitItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> DestProduitItem
        {
            get { return _destProduitItem; }
            set { _destProduitItem = value; }
        }

        /// <summary>
        /// Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
        /// </summary>
        /*public IEnumerable<SelectListItem> DestProduitItem
        {
            get
            {
                var allOrgs = ListeDestProduit.Select(f => new SelectListItem
                {
                    Value = f.id.ToString(),
                    Text = f.nom_destination
                });
                return DefaultDestProItem.Concat(allOrgs);
            }
        }

        /// <summary>
        /// Entete dropdownlist destinaison produit
        /// </summary>
        public IEnumerable<SelectListItem> DefaultDestProItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Options -"
                }, count: 1);
            }
        }*/

        #endregion

        /// <summary>
        /// string à true ou false pour indiquer si le transport depend du STLO ou Autre
        /// </summary>
        [Required]
        [Display(Name = "Transport assuré par*: ")]
        public string TransportSTLO { get; set; }

        /// <summary>
        /// String pour rajouter un commentaire dans le formulaire
        /// </summary>
        [Display(Name = "Commentaire Essai")]
        public string CommentaireEssai { get; set; }

        #endregion

    }

    #region Model view pour la vue "PlanZonesReservation"
    /// <summary>
    /// View model pour la vue de selection des zones pour réservation
    /// </summary>
    public class ZonesReservationViewModel
    {
        #region Sélection des zones pour réservation 

        public EnumZonesPfl EnumZones { get; set; }

        private List<zone> _zones;
        /// <summary>
        /// Liste des zones de la plateforme récupérés à partir de la base de données
        /// </summary>
        public List<zone> Zones
        {
            get { return _zones;}
            set { _zones = value; }
        }
        #endregion

        private List<EquipementsParZoneViewModel> _equipementsParZone = new List<EquipementsParZoneViewModel>();
        /// <summary>
        /// Liste des équipements par zone déterminée à complèter après click sur une des zones
        /// </summary>
        public List<EquipementsParZoneViewModel> EquipementsParZone
        {
            get
            {
                return _equipementsParZone;
            }
            set { _equipementsParZone = value; }
        }
    }

    #endregion

    #region Model View pour la vue "EquipementsVsZone"
    /// <summary>
    /// View model qui répresente les équipements contenus dans une zone specifique
    /// </summary>
    public class EquipementsParZoneViewModel
    {
        /// <summary>
        /// Nom de la zone à reserver
        /// </summary>
        public string NomZone { get; set; }

        private List<equipement> _equipements;
        /// <summary>
        /// Liste des équipements par zone déterminée à complèter après click sur une des zones
        /// </summary>
        public List<equipement> Equipements
        {
            get
            {
                return _equipements;
            }
            set { _equipements = value; }
        }

        /// <summary>
        /// Id de la zone à réserver
        /// </summary>
        public int IdZone { get; set; }

        /// <summary>
        /// création du model child pour le passer à la vue partielle _CalendrierEquipement (tous les réservations par équipement)
        /// </summary>
        public List<CalendrierEquipChildViewModel> CalendrierChildVM { get; set; }

        public int IndiceChildModel { get; set; }

        public int IndiceResaEquipXChild { get; set; }

    }

    #endregion

    #region Model view pour les vues partielles "_Creneaux" et "_CalendrierEquipement"

    /// <summary>
    /// View model representant les infos sur les réservations d'un équipement specifique
    /// </summary>
    public class CalendrierEquipChildViewModel
    {
        /// <summary>
        /// informations détaillées pour chaque jour pour une semaine ou pour une durée déterminée par l'utilisateur
        /// </summary>
        public List<ReservationsJour> ListResas = new List<ReservationsJour>();

        /// <summary>
        /// Objet Equipement pour affichage et applications des opérations de réservation
        /// </summary>
        public equipement EquipementCalendrier { get; set; }

        /// <summary>
        /// Date debut à afficher pour le planning équipement 
        /// </summary>
        [Required]
        [Display(Name = "Du : ")]
        public DateTime? DatePickerDu { get; set; }

        /// <summary>
        /// Date fin pour afficher le planning equipement 
        /// </summary>
        [Required]
        [Display(Name = "Au : ")]
        public DateTime? DatePickerAu { get; set; }

        /// <summary>
        /// Date debut pour créneau réservation
        /// </summary>
        [Required]
        [Display(Name = "Date Debut : ")]
        public DateTime? DateDebut { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation
        /// </summary>
        [Required]
        [Display(Name = "Date Fin : ")]
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Définition créneau pour chaque datepicker (réservation)
        /// </summary>
        [Required]
        [Display(Name = "Créneau début")]
        public bool? DatePickerDebut_Matin { get; set; }

        [Required]
        [Display(Name = "Créneau fin")]
        public bool? DatePickerFin_Matin { get; set; }


        private List<ReservationTemp> _resaEquipement = new List<ReservationTemp>();
        /// <summary>
        /// Reservation saisie pour l'équipement 
        /// </summary>
        public List<ReservationTemp> ResaEquipement
        {
            get
            {
                return _resaEquipement;
            }
            set { _resaEquipement = value; }
        }

    }

    #endregion


    #region Enum pour établir des valeurs définis
    
    public enum EnumStatusEssai
    {
        WaitingValidation,
        Validate,
        Refuse,
        Canceled
    }

    public enum EnumZonesPfl
    {
        HaloirAp7 = 14,
        SalleAp6 = 13,
        SalleAp5 = 12,
        SalleAp9 = 16,
        SalleAp8 = 15,
        Saumurage = 10,
        PatesMoulage = 5,
        PatesTranchage = 6,
        Labo = 11,
        PatesCuites = 7, 
        PrepLait = 3,
        Membranes = 4,
        Innovation = 8,
        EquipMobiles = 17,
        Depot_Stockage = 2,
        Concent_Sech = 1,
        SalleStephan = 9
    }

    public enum EnumConfidentialite
    {
        Ouvert,
        Restreint,
        Confidentiel
    }
    #endregion

}