using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
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

        public List<CapteurXRapport> ListRapports()
        {
            CapteurXRapport captXrapp = new CapteurXRapport();
            List<CapteurXRapport> list = new List<CapteurXRapport>();

            var rapports = context.rapport_metrologie.Distinct().ToList();
            foreach (var x in rapports)
            {
                var capt = context.capteur.First(c => c.id == x.capteurID);
                var equip = context.equipement.First(e => e.id == capt.equipementID);

                captXrapp = new CapteurXRapport
                {
                    idCapteur = capt.id,
                    nomCapteur = capt.nom_capteur,
                    nomEquipement = equip.nom,
                    numGmao = equip.numGmao,
                    dateVerif = x.date_verif_metrologie,
                    nom_document = x.nom_document,
                    idRapport = x.id,
                    TypeRapport = x.type_rapport_metrologie,
                    Commentaire = x.commentaire,
                    CodeCapteur = capt.code_capteur
                };
                list.Add(captXrapp);
            }
            return list;
        }
    }
}
