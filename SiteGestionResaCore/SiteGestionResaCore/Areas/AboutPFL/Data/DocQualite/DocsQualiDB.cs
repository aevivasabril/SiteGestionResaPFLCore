using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                    NomRubriqueDoc = doc.nom_rubrique_doc,
                    DescriptionDoc = doc.description_doc_qualite
                });
            }
            return listXVue;
        }

        public string GetNomDoc(string cheminDoc)
        {
            // Appliquer une regex pour extraire uniquement le nom
            string regexPatt = @"([^\\s]+)$";

            Regex Rg = new Regex(regexPatt);
            MatchCollection match = Rg.Matches(cheminDoc);

            return match[0].Groups[1].Value;
        }
        public doc_qualite ObtenirDocAQ(int IdDoc)
        {
            return context.doc_qualite.First(d => d.id == IdDoc);
        }
    }
}
