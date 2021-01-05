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

        public IList<zone> GetAllZones()
        {
            return resaDB.zone.ToList();
        }

    }
}
