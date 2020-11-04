using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using SiteReservationGestionPFL.Areas.Equipe.Data;
using SiteReservationGestionPFL.Areas.Reservation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteReservationGestionPFL.Models
{
    public interface IResaDB: IDisposable
    {
        #region Création de compte
        List<organisme> ObtenirListOrg();

        void CreerUtilisateur(string nom, string prenom, int organismeId, string email);

        #endregion

        #region Gestion des utilisateurs
        List<utilisateur> ObtenirListAdmins();

        //Task<IList<utilisateur>> ObtenirListAdminsLogistiqueAsync();

        listAutresUtilisateurs ObtenirListAutres();

        utilisateur ObtenirUtilisateur(int id);

        IEnumerable<SelectListItem> ListUsersToSelectItem(List<utilisateur> utilisateurs);

        IEnumerable<SelectListItem> ListEssaisToSelectItem(List<EssaiUtilisateur> essais);

        IEnumerable<SelectListItem> ListTypeProjetItem(List<ld_type_projet> typeProj);

        IEnumerable<SelectListItem> ListFinancementItem(List<ld_financement> financement);

        IEnumerable<SelectListItem> ListOrgItem(List<organisme> organismes);

        IEnumerable<SelectListItem> ListRespItem(List<utilisateur> respProj);

        IEnumerable<SelectListItem> ListProveItem(List<ld_provenance> provProj);

        IEnumerable<SelectListItem> ListManipItem(List<utilisateur> manipulateur);

        IEnumerable<SelectListItem> ListProdEntreeItem(List<ld_produit_in> produitEnt);

        IEnumerable<SelectListItem> ListProvProdItem(List<ld_provenance_produit> provProduit);

        IEnumerable<SelectListItem> ListDestProdItem(List<ld_destination> destProduit);

        Task ChangeAccesToUser(int id);

        Task ChangeAccesToAdminAsync(int id);

        void ValidateAccount (int id);

        int IdAspNetUser(int id);

        int IdAspNetUserFromMail(string mail);

        Task DeleteRequestAccount(int id);

        Task AddAdminToLogisticRoleAsync(int id);

        Task RemoveLogisticRoleAsync(int id);

        Task<IList<utilisateur>> ObtenirUsersLogisticAsync();

        #endregion

        #region Réservation 

        List<ld_type_projet> ObtenirList_TypeProjet();

        List<ld_financement> ObtenirList_Financement();

        List<utilisateur> ObtenirList_UtilisateurValide();

        List<ld_provenance> ObtenirList_ProvenanceProjet();

        List<ld_produit_in> ObtenirList_TypeProduitEntree();

        List<ld_provenance_produit> ObtenirList_ProvenanceProduit();

        List<ld_destination> ObtenirList_DestinationPro();

        projet CreationProjet(string TitreProjet, int typeProjetId, int financId, int orgId,
            int respProjetId, string numProj, int provProj, string description, DateTime dateCreation, utilisateur User);

        essai CreationEssai(projet pr, utilisateur Usr, DateTime myDateTime, string confidentialite, int manipId, int ProdId, string precisionProd, string QuantProd,
                        int ProvId, int destProduit, string TransStlo, string commentaire);

        reservation_projet CreationReservation(equipement Equip, essai Essai, DateTime dateDebut, DateTime dateFin);

        bool ProjetExists(string NumeroProjet);

        Task<bool> VerifPropieteProjetAsync(string numProjet, utilisateur usr);

        List<EssaiUtilisateur> ObtenirList_EssaisUser(string NumeroProjet);

        projet ObtenirProjet_pourCopie(string numProjet);

        essai ObtenirEssai_pourCopie(int essaiID);

        int IdTypeProjetPourCopie(int IdProjet);
        //void CreationReservation();

        int IdFinancementPourCopie(int IdProjet);

        int IdRespoProjetPourCopie(int IdProjet);

        int IdProvenancePourCopie(int IdProjet);

        int IdProvProduitPourCopie(int IdEssai);

        int IdDestProduitPourCopie(int IdEssai);

        int IdProduitInPourCopie(int IdEssai);

        List<zone> ListeZones();

        List<equipement> ListeEquipements(int idZone);

        string GetNomZone(int idZone);

        string GetNomEquipement(int idEquip);

        ReservationsJour ObtenirReservationsJourXEquipement(DateTime dateResa, int IdEquipement);

        ReservationsJour ObtenirReservationsJourEssai(DateTime dateResa, int IdEquipement);

        equipement GetEquipement(int idEquip);

        void UpdateEssai(essai Essai, DateTime dateInf, DateTime dateSup);

        #endregion
    }
}
