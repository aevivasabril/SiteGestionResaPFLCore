using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public class RapportDB: IRapportDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<IRapportDB> logger;

        public RapportDB(
            GestionResaContext contextDb,
            ILogger<RapportDB> logger)
        {
            this.contextDb = contextDb;
            this.logger = logger;
        }

        /// <summary>
        /// Obtenir la liste des équipements qui ont au moins un capteur de déclaré
        /// </summary>
        /// <returns></returns>
        public IList<EquipVsCapteur> GetEquipements()
        {
            List<EquipVsCapteur> List = new List<EquipVsCapteur>();

            var equips = (from equip in contextDb.equipement
                           from capt in contextDb.capteur
                           where (equip.id == capt.equipementID)
                           select equip).Distinct().ToList();
            foreach(var eq in equips)
            {
                var listCapteurs = contextDb.capteur.Where(c => c.equipementID == eq.id).Distinct().ToList();
                foreach(var capt in listCapteurs)
                {
                    EquipVsCapteur eqCapt = new EquipVsCapteur { capteurId = capt.id, nomCapteur = capt.nom_capteur, equipementId = eq.id, nomEquipement = eq.nom, numGmao = eq.numGmao };
                    List.Add(eqCapt);
                }
            }

            return List;
        }
    }
}
