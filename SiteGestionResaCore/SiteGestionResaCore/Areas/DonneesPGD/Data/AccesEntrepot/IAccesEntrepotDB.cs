using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot
{
    public interface IAccesEntrepotDB
    {
        public List<EntrepotsXProjet> ObtenirListProjetsAvecEntrepotCree(utilisateur user);
        public List<EssaiAvecEntrepotxProj> ObtenirListEssaiAvecEntrepot(int IdProjet);
        public string ObtNumProjet(int IdProjet);
        public List<DocXEssai> ObtListDocsXEssai(int IdEssai);
        public string ObtTitreEssai(int IdEssai);
        public int RecupIdEssaiXDoc(int IdDoc);
        public bool SupprimerDocument(int IdDoc);
        public bool DocSupprimable(int IdDoc);
        public List<essai> ListEssaiEntrepotxProjet(int IdProjet);
        public List<doc_essai_pgd> ListDocsEssai(int IdEssai);
        public string CorrigerStringNomDossier(string NomDossier);
        public projet GetProjet(int IdProjet);
        public bool CreateDirectoryTemp(string path);
    }
}
