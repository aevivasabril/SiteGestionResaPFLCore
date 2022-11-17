using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data
{
    public interface IDocMetroDB
    {
        bool AddingDocMetrologie(DocumentMetrologie doc);

        List<DocumentMetrologie> GetListDocuments();

        doc_metrologie ObtenirDocMetro(int id);

        bool SupprimerDoc(int id);
    }
}
