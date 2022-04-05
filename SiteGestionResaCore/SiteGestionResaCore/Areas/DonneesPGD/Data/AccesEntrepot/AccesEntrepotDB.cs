using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot
{
    public class AccesEntrepotDB: IAccesEntrepotDB
    {
        private readonly GestionResaContext contextDB;
        private readonly ILogger<AccesEntrepotDB> logger;

        public AccesEntrepotDB(
            GestionResaContext contextDB,
            ILogger<AccesEntrepotDB> logger)
        {
            this.contextDB = contextDB;
            this.logger = logger;
        }
        public List<EntrepotsXProjet> ObtenirListProjetsAvecEntrepotCree(utilisateur user)
        {
            List<EntrepotsXProjet> list = new List<EntrepotsXProjet>();
            var projetXUser = contextDB.projet.Where(p => p.compte_userID == user.Id);
            foreach(var x in projetXUser)
            {

                if(contextDB.essai.Where(e => e.projetID == x.id).ToList().Any(e=>e.entrepot_cree == true))
                {
                    EntrepotsXProjet entre = new EntrepotsXProjet { NomProjet = x.titre_projet, NumProjet = x.num_projet, IdProjet = x.id };
                    list.Add(entre);
                }
            }
            return list;
        }

        public List<EssaiAvecEntrepotxProj> ObtenirListEssaiAvecEntrepot(int IdProjet)
        {
            List<EssaiAvecEntrepotxProj> ListEssai = new List<EssaiAvecEntrepotxProj>();
            var essais = contextDB.essai.Where(e => e.projetID == IdProjet && e.entrepot_cree == true).ToList();
            foreach(var essai in essais)
            {
                EssaiAvecEntrepotxProj EssaiAvecEnt = new EssaiAvecEntrepotxProj { IdEssai = essai.id, TitreEssai = essai.titreEssai };
                ListEssai.Add(EssaiAvecEnt);
            }
            return ListEssai;
        }
        public string ObtNumProjet(int IdProjet)
        {
            return contextDB.projet.First(p => p.id == IdProjet).num_projet;
        }

        public List<DocXEssai> ObtListDocsXEssai(int IdEssai)
        {
            List<DocXEssai> listDocs = new List<DocXEssai>();
            DocXEssai doc = new DocXEssai();
            var list = contextDB.doc_essai_pgd.Where(d => d.essaiID == IdEssai).ToList();
            foreach(var l in list)
            {
                if(l.equipementID != null)
                {
                    doc = new DocXEssai
                    {
                        IdDoc = l.id,
                        IdEquipement = l.equipementID.Value,
                        NomEquipement = contextDB.equipement.FirstOrDefault(e => e.id == l.equipementID).nom,
                        TypeDonnees = contextDB.type_document.First(t => t.id == l.type_documentID).nom_document,
                        TypeActivite = contextDB.activite_pfl.First(a => a.id == l.type_activiteID).nom_activite,
                        NomDocument = l.nom_document
                    };
                }
                else
                {
                    doc = new DocXEssai
                    {
                        IdDoc = l.id,
                        TypeDonnees = contextDB.type_document.First(t => t.id == l.type_documentID).nom_document,
                        TypeActivite = contextDB.activite_pfl.First(a => a.id == l.type_activiteID).nom_activite,
                        NomDocument = l.nom_document
                    };
                    
                }
                
                listDocs.Add(doc);
            }

            return listDocs;
        }

        public string ObtTitreEssai(int IdEssai)
        {
            return contextDB.essai.First(e => e.id == IdEssai).titreEssai;
        }

        public int RecupIdEssaiXDoc(int IdDoc)
        {
            return contextDB.doc_essai_pgd.First(d => d.id == IdDoc).essaiID;
        }

        public bool SupprimerDocument(int IdDoc)
        {
            doc_essai_pgd doc = new doc_essai_pgd();
            try
            {
                doc = contextDB.doc_essai_pgd.First(r => r.id == IdDoc);
                contextDB.doc_essai_pgd.Remove(doc);
                contextDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la suppression du document: " + doc.nom_document);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Methode pour vérifier qu'il ne s'agit pas d'un fichier géneré automatiquement
        /// </summary>
        /// <param name="IdDoc">id document</param>
        /// <returns></returns>
        public bool DocSupprimable(int IdDoc)
        {
            string docPcVue = @"^DonneesProjet_";
            string docTxt = @"^Informations_Essai_[0-9]*";
            // vérifier s'il s'agit de la doc informations essai
            var doc = contextDB.doc_essai_pgd.First(d => d.id == IdDoc);
            Regex RgTxt = new Regex(docTxt);
            bool IsTrueTxt = RgTxt.IsMatch(doc.nom_document);
            // Vérifier s'il s'agit de l'excel PcVue
            Regex RgPcVue = new Regex(docPcVue);
            bool IsTruePcvue = RgPcVue.IsMatch(doc.nom_document);

            if (IsTrueTxt == true || IsTruePcvue == true)
                return false;
            else
                return true;
        }

        public List<essai> ListEssaiEntrepotxProjet(int IdProjet)
        {
            return contextDB.essai.Where(e => e.entrepot_cree == true && e.projetID == IdProjet).ToList();
        }
        public List<doc_essai_pgd> ListDocsEssai(int IdEssai)
        {
            return contextDB.doc_essai_pgd.Where(d => d.essaiID == IdEssai).ToList();
        }

        public string CorrigerStringNomDossier(string NomDossier)
        {
            string pattern = @"[\\~#%&*{()}\/:;,<>?|\"" ]";
            string remplacement = "_";
            Regex rg = new Regex(pattern);
            string str = rg.Replace(NomDossier, remplacement);
            return str;
        }

        public projet GetProjet(int IdProjet)
        {
            return contextDB.projet.First(p => p.id == IdProjet);
        }

        public bool CreateDirectoryTemp(string path)
        {
            try
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la création du dossier : " + path + ".Problème: "+ e.ToString());
                return false;
            }
            return true;
        }

        public activite_pfl ObtActivite(int IdActivite)
        {
            return contextDB.activite_pfl.First(a => a.id == IdActivite);
        }

        public equipement GetEquipement(int IdEquip)
        {
            return contextDB.equipement.First(e => e.id == IdEquip);
        }
        public string TraiterChaineCaract(string titre, int taille)
        {
            string stringModifie = "";

            #region Vérification de la taille pour le limiter

            if (titre.Length > taille)
                stringModifie = titre.Substring(0, taille);
            else
                stringModifie = titre;
            #endregion

            #region Supprimer les accents d'un string

            byte[] bytesP = System.Text.Encoding.GetEncoding(1251).GetBytes(stringModifie);
            stringModifie = System.Text.Encoding.ASCII.GetString(bytesP);
            #endregion

            // limiter les noms des dossiers car sinon trop longs
            stringModifie =CorrigerStringNomDossier(stringModifie);

            return stringModifie;
        }

        public type_document ObtenirTypeDocument(int idTypeDoc)
        {
            return contextDB.type_document.First(d => d.id == idTypeDoc);
        }

        /*public bool SupprimerEntrepotxProjet(int IdProjet)
        {
            bool IsOk = false;
            // Obtenir essais du projet où un entrepôt des données a été créé
            var essais = contextDB.essai.Where(e => e.projetID == IdProjet && e.entrepot_cree == true).ToList();
            // pour chaque essai, supprimer les documents associés
            foreach(var ess in essais)
            {
                var docList = contextDB.doc_essai_pgd.Where(d => d.essaiID == ess.id).ToList();
                
                foreach(var dc in docList)
                {
                    try
                    {
                        contextDB.doc_essai_pgd.Remove(dc);
                        contextDB.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        logger.LogError(e.ToString(), "Problème lors de la suppression du document");
                        return false;
                    }
                }
                // Signaler que l'essai possède un entrepot des données supprimé
                ess.entrepot_supprime = true;
                contextDB.SaveChanges();
            }
            IsOk = true;


            return true;
        }*/
    }
}
