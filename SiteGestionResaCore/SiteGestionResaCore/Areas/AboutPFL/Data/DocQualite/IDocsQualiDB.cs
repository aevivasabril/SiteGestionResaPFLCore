using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite
{
    public interface IDocsQualiDB
    {
        List<DocumentQualite> ListDocs();
        string GetNomDoc(string cheminDoc);
        doc_qualite ObtenirDocAQ(int IdDoc);
        List<CapteurXRapport> ListRapports();
    }
}
