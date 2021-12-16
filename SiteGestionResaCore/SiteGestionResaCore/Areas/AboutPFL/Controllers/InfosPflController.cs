using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Controllers
{
    [Area("AboutPFL")]
    public class InfosPflController : Controller
    {
        private readonly IZonePflEquipDB pflEquipDB;

        public InfosPflController(
            IZonePflEquipDB pflEquipDB)
        {
            this.pflEquipDB = pflEquipDB;
        }

        /// <summary>
        /// Ouverture de la vue pour affichage du plan PFL
        /// </summary>
        /// <returns></returns>
        public IActionResult PlanZones()
        {
            ZonesPflViewModel vm = new ZonesPflViewModel()
            {
                Zones = pflEquipDB.ListeZones()
            };

            return View("PlanZonesPFL",vm);
        }

        public IActionResult ZoneVsEquipements(int ?id)
        {
            List<InfosEquipement> listEquipVM = new List<InfosEquipement>();
            //List<equipement> equipements = new List<equipement>();
            // 1. Obtenir la liste des equipements
            listEquipVM = pflEquipDB.ListeEquipementsXZone(id.Value);
            // 2. For pour crée la liste des InfosEquipement
            
            string nomZone = pflEquipDB.NomZoneXEquipement(id.Value);
            EquipParZoneViewModel vm = new EquipParZoneViewModel()
            {
               ListeEquipements = listEquipVM,
               NomZone = nomZone
            };

            return View("EquipsVsZone", vm);
        }

        public IActionResult DownloadFichMat(int? id)
        {
            string cheminFichier = pflEquipDB.GetCheminFicheMateriel(id.Value);
            string NomFichier = pflEquipDB.GetNomXChemin(cheminFichier);
            byte[] fileBytes = System.IO.File.ReadAllBytes(cheminFichier);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, NomFichier);
        }
    }
}
