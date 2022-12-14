using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    public class CapteurDB: ICapteurDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<CapteurDB> logger;

        public CapteurDB(
            GestionResaContext contextDb,
            ILogger<CapteurDB> logger)
        {
            this.contextDb = contextDb;
            this.logger = logger;
        }

        public List<CapteurXAffichage> ObtenirListCapteurs()
        {
            List<CapteurXAffichage> listCapteur = new List<CapteurXAffichage>();
            var list = contextDb.capteur.ToList();

            foreach (var l in list)
            {
                equipement eq = contextDb.equipement.First(e => e.id == l.equipementID);
                CapteurXAffichage cap = new CapteurXAffichage
                {
                    idCapteur = l.id,
                    NomCapteur = l.nom_capteur,
                    NomPilote = eq.nom
                };

                listCapteur.Add(cap);
            }

            return listCapteur;
        }

        public List<equipement> ObtListEquipements()
        {
            return contextDb.equipement.ToList();
        }

        public bool AjouterCapteur(string NomCapteur, string CodeCapteur, int SelectedPiloteID, DateTime DateProchaineVerif,
                    DateTime DateDernierVerif, double period, bool CapteurConforme, double EmtCapteur, double FacteurCorrectif)
        {
            capteur capt = new capteur();
            equipement equip = contextDb.equipement.First(e=>e.id == SelectedPiloteID);
            if(DateDernierVerif.Year == 0001)
            {
                DateDernierVerif = DateDernierVerif.AddYears(1753); // SQL accepte des dates à partir de l'année 1753 sinon on a une erreur
            }

            capt = new capteur
            {
                nom_capteur = NomCapteur,
                code_capteur = CodeCapteur,
                equipementID = SelectedPiloteID,
                date_prochaine_verif = DateProchaineVerif,
                date_derniere_verif = DateDernierVerif,
                periodicite_metrologie = period,
                capteur_conforme = CapteurConforme,
                emt_capteur = EmtCapteur,
                facteur_correctif = FacteurCorrectif
            };

            try
            {
                contextDb.capteur.Add(capt);
                contextDb.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                logger.LogError("", "problème lors de la sauvegarde du capteur: " + e.ToString());
                return false;
            }
        }

        public capteur ObtenirCapteur(int id)
        {
            return contextDb.capteur.First(c => c.id == id);
        }

        public bool SupprimerCapteur(int idCapteur)
        {
            bool isOk = false;
            capteur capt = new capteur();
            try
            {
                capt = contextDb.capteur.First(d => d.id == idCapteur);
                contextDb.capteur.Remove(capt);
                contextDb.SaveChanges();
                isOk = true;
            }
            catch (Exception e)
            {
                isOk = false;
                logger.LogError("", "Problème pour supprimer le capteur: " + capt.nom_capteur + "Erreur: " + e.ToString());
            }

            return isOk;
        }

        public equipement ObtenirEquipement(int idEquipement)
        {
            return contextDb.equipement.First(e => e.id == idEquipement);
        }

        public bool UpdatePeriodicite(capteur capt, double periodicite)
        {
            try
            {
                capt.periodicite_metrologie = periodicite;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la periodicité d'un capteur");
                return false;
            }
            return true;
        }

        public bool UpdateDateProVerif(capteur capt, DateTime dateverif)
        {
            try
            {
                capt.date_prochaine_verif = dateverif;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la date prochaine vérification");
                return false;
            }
            return true;
        }

        public bool UpdateDateDerniereVerif(capteur capt, DateTime dateverif)
        {
            try
            {
                capt.date_derniere_verif = dateverif;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la date dernière vérification");
                return false;
            }
            return true;
        }

        public bool UpdateFacteur(capteur capt, double facteur)
        {
            try
            {
                capt.facteur_correctif = facteur;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ du facteur de correction");
                return false;
            }
            return true;
        }

        public bool UpdateEMT(capteur capt, double emt)
        {
            try
            {
                capt.emt_capteur = emt;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de l'EMT capteur");
                return false;
            }
            return true;
        }

        public bool UpdateCodeCapteur(capteur capt, string code)
        {
            try
            {
                capt.code_capteur = code;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ du code capteur");
                return false;
            }
            return true;
        }

       public bool UpdateNomCapteur(capteur capt, string nom)
        {
            try
            {
                capt.nom_capteur = nom;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ du nom capteur");
                return false;
            }
            return true;
        }

        public bool UpdateConformite(capteur capt, bool conformite)
        {
            try
            {
                capt.capteur_conforme = conformite;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ conformité capteur");
                return false;
            }
            return true;
        }

        public double FacteurCorrectif(capteur capt)
        {
            return contextDb.capteur.First(c => c.id == capt.id).facteur_correctif.Value;
        }
    }
}
