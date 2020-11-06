using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public interface IZoneEquipDb: IDisposable
    {
        List<zone> ListeZones();

        List<equipement> ListeEquipements(int idZone);

        equipement GetEquipement(int idEquip);

        string GetNomZone(int idZone);

      
    }
}
