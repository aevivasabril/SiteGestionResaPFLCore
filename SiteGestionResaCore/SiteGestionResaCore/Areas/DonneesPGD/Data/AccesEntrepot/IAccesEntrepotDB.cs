using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot
{
    public interface IAccesEntrepotDB
    {
        List<EntrepotsXProjet> ObtenirListProjetsAvecEntrepotCree(utilisateur user);
        List<EssaiAvecEntrepotxProj> ObtenirListEssaiAvecEntrepot(int IdProjet);
        string ObtNumProjet(int IdProjet);
        List<DocXEssai> ObtListDocsXEssai(int IdEssai);
        string ObtTitreEssai(int IdEssai);
        int RecupIdEssaiXDoc(int IdDoc);
        bool SupprimerDocument(int IdDoc);
        bool DocSupprimable(int IdDoc);
        List<essai> ListEssaiEntrepotxProjet(int IdProjet);
        List<doc_essai_pgd> ListDocsEssai(int IdEssai);
        string CorrigerStringNomDossier(string NomDossier);
        projet GetProjet(int IdProjet);
        bool CreateDirectoryTemp(string path);
        activite_pfl ObtActivite(int IdActivite);
        equipement GetEquipement(int IdEquip);
        string TraiterChaineCaract(string titre, int taille);
        type_document ObtenirTypeDocument(int idTypeDoc);
        bool SupprimerEntrepotXProjet(int IdProjet);
        double CalculTotalKoEntrepot(int idProjet);
        List<EntrepotsXProjet> ObtenirListTousEntrepots();
    }
}
