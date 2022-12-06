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
    }
}
