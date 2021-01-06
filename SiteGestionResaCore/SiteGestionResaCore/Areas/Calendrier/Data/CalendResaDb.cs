using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public class CalendResaDb: ICalendResaDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private readonly GestionResaContext resaDB;
        private readonly ILogger<CalendResaDb> logger;

        public CalendResaDb(
            GestionResaContext resaDB,
            ILogger<CalendResaDb> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        public List<InfosCalenZone> ObtenirZonesVsEquipements()
        {
            List<InfosCalenZone> listInfos = new List<InfosCalenZone>();
            InfosCalenZone infosTemp = new InfosCalenZone();
            List<string> NomsEquip = new List<string>();

            var zones = resaDB.zone.ToList();

            foreach(var zo in zones)
            {
                infosTemp = new InfosCalenZone()
                {
                    IdZone = zo.id,
                    NomZone = zo.nom_zone,
                    ListEquipements = resaDB.equipement.Where(e => e.zoneID == zo.id).Select(e => e.nom).ToList()
                };
                listInfos.Add(infosTemp);
            }   
            return listInfos;
        }

    }
}
