using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq
{
    public interface IModifAqDB
    {
        public List<DocQualiteToModif> ObtenirDocsAQXModif();

        public bool AjouterDocToBDD(DocQualiteToModif doc);

        public doc_qualite ObtenirDocQualite(int IdDoc);

        public bool SupprimerDocAQ(int IdDoc);
    }
}
