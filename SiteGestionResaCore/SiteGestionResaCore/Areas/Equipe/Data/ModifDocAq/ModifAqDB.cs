using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq
{
    public class ModifAqDB : IModifAqDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<ModifAqDB> logger;

        public ModifAqDB(
            GestionResaContext contextDb,
            ILogger<ModifAqDB> logger)
        {
            this.contextDb = contextDb;
            this.logger = logger;
        }

        public List<DocQualiteToModif> ObtenirDocsAQXModif()
        {
            List<DocQualiteToModif> list = new List<DocQualiteToModif>();
            var sublist = contextDb.doc_qualite.ToList();
            foreach(var d in sublist)
            {
                DocQualiteToModif doc = new DocQualiteToModif
                {
                    IdDoc = d.id,
                    NomDocument = d.nom_document,
                    DernierDateModif = d.date_modif_doc.Value,
                    DescripDoc = d.description_doc_qualite,
                    NomRubriqDoc = d.nom_rubrique_doc
                };
                list.Add(doc);
            }
            return list;
        }

        public bool AjouterDocToBDD(DocQualiteToModif doc)
        {
            bool isOk = false;
            try
            {
                doc_qualite docQ = new doc_qualite
                {
                    contenu_doc_qualite = doc.ContenuDoc,
                    date_modif_doc = doc.DernierDateModif,
                    description_doc_qualite = doc.DescripDoc,
                    nom_document = doc.NomDocument,
                    nom_rubrique_doc = doc.NomRubriqDoc
                };
                contextDb.doc_qualite.Add(docQ);
                contextDb.SaveChanges();
                isOk = true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Erreur d'écriture du document dans la table doc_essai_pgd: " + e.ToString());
                isOk = false;
            }

            return isOk;
        }

        public doc_qualite ObtenirDocQualite(int id)
        {
            return contextDb.doc_qualite.First(d => d.id == id);
        }

        public bool SupprimerDocAQ(int IdDoc)
        {
            bool IsOk = false;
            try
            {
                doc_qualite doc = contextDb.doc_qualite.First(d => d.id == IdDoc);
                contextDb.doc_qualite.Remove(doc);
                contextDb.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Erreur lors de la suppression du document qualité: "+ e.ToString());
                IsOk = false;
            }
            return IsOk;
        }
    }
}
