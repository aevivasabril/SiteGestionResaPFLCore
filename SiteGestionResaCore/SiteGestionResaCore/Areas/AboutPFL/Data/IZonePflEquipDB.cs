using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data
{
    public interface IZonePflEquipDB
    {
        List<zone> ListeZones();
        List<equipement> ListeEquipementsXZone(int idZone);
        string NomZoneXEquipement(int idZone);
    }
}
