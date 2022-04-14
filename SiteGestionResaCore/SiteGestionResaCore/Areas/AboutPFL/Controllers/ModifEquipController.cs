using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data;
using SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Controllers
{
    [Area("AboutPFL")]
    public class ModifEquipController : Controller
    {
        private readonly IZonePflEquipDB pflEquipDB;

        public ModifEquipController(
            IZonePflEquipDB pflEquipDB)
        {
            this.pflEquipDB = pflEquipDB;
        }

        public IActionResult GestionEquipements()
        {
            ZonesPflViewModel vm = new ZonesPflViewModel()
            {
                Zones = pflEquipDB.ListeZones()
            };
            return View("GestionEquips", vm);
        }

        public IActionResult ZoneVsEquipXmodif(int? id)
        {
            List<InfosEquipement> listEquipVM = new List<InfosEquipement>();
            //List<equipement> equipements = new List<equipement>();
            // 1. Obtenir la liste des equipements
            listEquipVM = pflEquipDB.ListeEquipementsXZone(id.Value);
            // 2. For pour crée la liste des InfosEquipement

            string nomZone = pflEquipDB.NomZoneXEquipement(id.Value);
            EquipsXModifVM vm = new EquipsXModifVM()
            {
                ListeEquipements = listEquipVM,
                NomZone = nomZone
            };

            return View("EquipsXModif", vm);
        }
    }
}
