using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq
{
    public interface IModifAqDB
    {
        List<DocQualiteToModif> ObtenirDocsAQXModif();
        bool AjouterDocToBDD(DocQualiteToModif doc);
        doc_qualite ObtenirDocQualite(int IdDoc);
        bool SupprimerDocAQ(int IdDoc);
    }
}
