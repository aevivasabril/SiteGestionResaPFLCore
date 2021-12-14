using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data
{
    public class ZonePflEquipDB : IZonePflEquipDB
    {
        private readonly GestionResaContext context;

        public ZonePflEquipDB(
            GestionResaContext context)
        {
            this.context = context;
        }
        public List<zone> ListeZones()
        {
            return context.zone.ToList();
        }

        /// <summary>
        /// Méthode pour obtenir une liste des équipements pour une zone déterminée
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>Liste des équipements + boolean si équipement réservé ou pas</returns>
        public List<equipement> ListeEquipementsXZone(int idZone)
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

        public string NomZoneXEquipement(int idZone)
        {
            return context.zone.First(z => z.id == idZone).nom_zone;
        }

    }
}
