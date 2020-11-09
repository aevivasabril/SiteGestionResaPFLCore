using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class ZoneEquipDb: IZoneEquipDb
    {

        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;
        private readonly ILogger<ZoneEquipDb> logger;

        /// <summary>
        /// Constructeur initializant la connexion vers les 2 base de données
        /// </summary>
        public ZoneEquipDb(
            GestionResaContext resaDB,
            UserManager<utilisateur> userManager,
            ILogger<ZoneEquipDb> logger)
        {
            this.context = resaDB;
            this.userManager = userManager;
            this.logger = logger;
        }
        /// <summary>
        /// Obtenir liste de toutes les zones PFL
        /// </summary>
        /// <returns>Liste "zone"</returns>
        public List<zone> ListeZones()
        {
            return context.zone.ToList();
        }

        /// <summary>
        /// Méthode pour obtenir une liste des équipement pour une zone déterminée
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>Liste des équipements + boolean si équipement réservé ou pas</returns>
        public List<equipement> ListeEquipements(int idZone)
        {
            List<equipement> List = new List<equipement>();

            var query = (from equip in context.equipement
                         where equip.zoneID == idZone
                         select equip).ToArray();

            foreach (var y in query)
            {
                List.Add(y);
            }

            return List;
        }

        public equipement GetEquipement(int IdEquip)
        {
            return (context.equipement.FirstOrDefault(u => u.id == IdEquip));
        }


        /// <summary>
        /// Méthode pour obtenir le nom d'une zone
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>nom de la zone</returns>
        public string GetNomZone(int idZone)
        {
            return context.zone.First(x => x.id == idZone).nom_zone;
        }





       
    }
}
