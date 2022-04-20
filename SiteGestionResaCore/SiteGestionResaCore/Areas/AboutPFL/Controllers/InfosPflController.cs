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

        /// <summary>
        /// Méthode pour télécharger la doc depuis la bases des données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DownloadFichMat(int? id)
        {
            doc_fiche_materiel doc = pflEquipDB.ObtenirDocMateriel(id.Value);
            return File(doc.contenu_fiche, System.Net.Mime.MediaTypeNames.Application.Octet, doc.nom_document);
        }
    }
}
