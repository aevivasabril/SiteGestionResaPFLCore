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

        public bool AjouterCapteur(AjouterCapteurVM model)
        {
            bool isOk = false;
            capteur capt = new capteur();
            equipement equip = contextDb.equipement.First(e=>e.id == model.SelectedPiloteID);

            //capt = new capteur { nom_capteur = model.NomCapteur, code_capteur = model.CodeCapteur, equipementID = model.SelectedPiloteID,  };

            return isOk;
        }
    }
}
