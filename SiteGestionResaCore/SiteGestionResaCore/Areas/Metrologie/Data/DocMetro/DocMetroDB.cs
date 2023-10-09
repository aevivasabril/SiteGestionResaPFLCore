using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Metrologie.Data.DocMetro;
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

        /// <summary>
        /// Recuperer les vérifications métrologiques 
        /// </summary>
        /// <returns></returns>
        public List<CapteurXVerif> ListProchainesVerifs()
        {
            List<CapteurXVerif> List = new List<CapteurXVerif>();
            List<capteur> CapteursAFaire = new List<capteur>();
            List<capteur> ListCaptsInternes = new List<capteur>();
            List<capteur> ListCaptsExternes = new List<capteur>();
            // Vérifier les opérations métrologiques à réaliser dans les 6 prochains mois
            
            // Prochaines vérifs internes
            List<capteur> ListCapts = contextDb.capteur.Distinct().ToList();

            string periodicite = "";
            rapport_metrologie rapport = new rapport_metrologie();

            foreach (var capt in ListCapts)
            {
                if(capt.date_prochaine_verif_int.Value < DateTime.Today.AddMonths(6)) // Si la prochaine vérif interne a lieu dans les prochains 6 mois alors sauvegarder
                {
                    // on peut car on aura qu'un rapport interne et un rapport externe par capteur
                    rapport = contextDb.rapport_metrologie.FirstOrDefault(r => r.capteurID == capt.id && r.type_rapport_metrologie == "Interne");
                    // si le rapport est null cela veut dire qu'il n'a pas de date de prochaine vérif interne pour ce capteur
                    if(rapport != null)
                    {
                        if (capt.periodicite_metrologie_int == 0.5)
                            periodicite = "6 mois";
                        else if (capt.periodicite_metrologie_int == 1)
                            periodicite = "1 an";
                        else
                            periodicite = "2 ans";

                        CapteurXVerif captXVer = new CapteurXVerif
                        {
                            CodeCapteur = capt.code_capteur,
                            DateDerniereVerif = rapport.date_verif_metrologie,
                            DateProVerif = capt.date_prochaine_verif_int.Value,
                            NomCapteur = capt.nom_capteur,
                            Periodicite = periodicite,
                            TypeVerif = rapport.type_rapport_metrologie
                        };

                        List.Add(captXVer);
                    }
                }
                if(capt.date_prochaine_verif_ext.Value < DateTime.Today.AddMonths(6)) // Si la prochaine vérif externe a lieu dans les prochains 6 mois alors sauvegarder
                {
                    rapport = contextDb.rapport_metrologie.FirstOrDefault(r => r.capteurID == capt.id && r.type_rapport_metrologie == "Externe");
                    // si le rapport est null cela veut dire qu'il n'a pas de date de prochaine vérif externe pour ce capteur
                    if (rapport != null)
                    {
                        if (capt.periodicite_metrologie_ext == 0.5)
                            periodicite = "6 mois";
                        else if (capt.periodicite_metrologie_ext == 1)
                            periodicite = "1 an";
                        else
                            periodicite = "2 ans";

                        CapteurXVerif captXVer = new CapteurXVerif
                        {
                            CodeCapteur = capt.code_capteur,
                            DateDerniereVerif = rapport.date_verif_metrologie,
                            DateProVerif = capt.date_prochaine_verif_ext.Value,
                            NomCapteur = capt.nom_capteur,
                            Periodicite = periodicite,
                            TypeVerif = rapport.type_rapport_metrologie
                        };

                        List.Add(captXVer);
                    }                   
                }
            }        
            return List;
        }
    }
}
