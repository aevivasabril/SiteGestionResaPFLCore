using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite
{
    public interface IDocsQualiDB
    {
        List<DocumentQualite> ListDocs();
        string GetCheminDocQualite(int id);
        string GetNomDoc(string cheminDoc);
    }
}
