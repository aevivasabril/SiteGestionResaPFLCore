using SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public interface IStatistiquesDB
    {
        List<InfosReservations> ObtenirResasDuAu(DateTime datedu, DateTime dateau);
        List<ZoneStats> ObtenirListZones();
        List<InfosEquipVsJours> ObtListEquipsVsJours(int idZone, int Annee);
        zone ObtNomZone(int id);
        List<projet> ObtenirListProjet();
        List<InfosReservations> ObtRecapitulatifXProjet(int IdProjet);
        projet ObtenirProjet(int IdProjet);
        List<ld_equipes_stlo> ObtenirListEquip();
        List<InfosReservations> ObtRecapitulatifXEquipe(int IdEquipeStlo, DateTime datedebut, DateTime datefin);
        ld_equipes_stlo ObtInfosEquipe(int IdEquipe);
        List<organisme> ListeOrganismes();
        List<InfosReservations> ObtRecapitulatifXOrg(int IdOrg, DateTime datedebut, DateTime datefin);
        organisme ObtenirOrganisme(int IdOrg);
        List<CategorieXProj> ListProjXProvenance(int IdProv);
        ld_provenance NomProvenance(int IdProvenance);
        List<ld_provenance> ListeProvenances();
        List<CategorieXProj> ListProjXNonProv();
        int LaitAnneeEnCours();
        int LaitXDates(DateTime dateDebut, DateTime dateFin);
        List<MaintenanceInfos> ListMaintenances(DateTime dateDu, DateTime dateAu);
        List<ld_type_projet> ListTypeProjet();
        List<CategorieXProj> ListProjetXType(int idType);
        ld_type_projet NomTypeProj(int Id);
        List<CategorieXProj> ListProjsSansType();
    }
}
