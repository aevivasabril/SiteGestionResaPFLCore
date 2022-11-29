using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data
{
    public class DocMetroDB : IDocMetroDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<DocMetroDB> logger;

        public DocMetroDB(
            GestionResaContext contextDb,
            ILogger<DocMetroDB> logger)
        {
            this.contextDb = contextDb;
            this.logger = logger;
        }

        public bool AddingDocMetrologie(DocumentMetrologie doc)
        {
            bool IsOk = false;
            try
            {
                doc_metrologie document = new doc_metrologie()
                {
                    contenu_doc = doc.ContenuDoc,
                    date_creation = doc.dateAjout,
                    description_doc = doc.DescriptionDoc,
                    nom_document = doc.NomDocument
                };
                contextDb.doc_metrologie.Add(document);
                contextDb.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Erreur d'écriture du document dans la table doc_metrologie: " + e.ToString());
                IsOk = false;
            }
            return IsOk;
        }

        public List<DocumentMetrologie> GetListDocuments()
        {
            List<DocumentMetrologie> list = new List<DocumentMetrologie>();
            var sublist = contextDb.doc_metrologie.ToList();
            foreach (var d in sublist)
            {
                DocumentMetrologie doc = new DocumentMetrologie
                {
                    idDoc = d.id,
                    NomDocument = d.nom_document,
                    dateAjout = d.date_creation,
                    DescriptionDoc = d.description_doc,
                    ContenuDoc = d.contenu_doc
                };
                list.Add(doc);
            }
            return list;
        }

        public doc_metrologie ObtenirDocMetro(int id)
        {
            return contextDb.doc_metrologie.First(d => d.id == id);
        }

        public bool SupprimerDoc(int id)
        {
            bool isOk = false;
            doc_metrologie doc = new doc_metrologie();
            try
            {
                doc = contextDb.doc_metrologie.First(i => i.id == id);
                contextDb.doc_metrologie.Remove(doc);
                contextDb.SaveChanges();
                isOk = true;
            }
            catch(Exception e)
            {
                isOk = false;
                logger.LogError("", "Problème pour supprimer le document: " + doc.nom_document + "Erreur: " + e.ToString());
            }

            return isOk;
        }
    }
}
