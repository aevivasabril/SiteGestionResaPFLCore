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
        List<InfosEquipement> ListeEquipementsXZone(int idZone);
        string NomZoneXEquipement(int idZone);
        doc_fiche_materiel ObtenirDocMateriel(int idDoc);
    }
}
