using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite
{
    public class DocsQualiDB : IDocsQualiDB
    {
        private readonly GestionResaContext context;

        public DocsQualiDB(
            GestionResaContext context)
        {
            this.context = context;
        }
        public List<DocumentQualite> ListDocs()
        {
            List<DocumentQualite> listXVue = new List<DocumentQualite>();
            var list = context.doc_qualite.ToList();
            foreach (var doc in list)
            {
                listXVue.Add(new DocumentQualite
                {
                    IdDocument = doc.id,
                    NomDocument = doc.nom_document,
                    CheminDocument = doc.chemin_document,
                    DescriptionDoc = doc.description_doc
                });
            }
            return listXVue;
        }
    }
}
