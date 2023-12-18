using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data;
using SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Controllers
{
    [Area("AboutPFL")]
    public class ModifEquipController : Controller
    {
        private readonly IZonePflEquipDB pflEquipDB;
        private readonly IEquipsToModifDB equipsToModifDB;

        public ModifEquipController(
            IZonePflEquipDB pflEquipDB,
            IEquipsToModifDB equipsToModifDB)
        {
            this.pflEquipDB = pflEquipDB;
            this.equipsToModifDB = equipsToModifDB;
        }

        public IActionResult GestionEquipements()
        {
            ZonesPflViewModel vm = new ZonesPflViewModel()
            {
                Zones = pflEquipDB.ListeZones()
            };
            return View("GestionEquips", vm);
        }

        /// <summary>
        /// Affichage des équipements avec leur fiche si dispo
        /// </summary>
        /// <param name="id">id zone</param>
        /// <returns></returns>
        public IActionResult ZoneVsEquipXmodif(int? id)
        {
            List<InfosEquipement> listEquipVM = new List<InfosEquipement>();
            //List<equipement> equipements = new List<equipement>();
            // 1. Obtenir la liste des equipements
            listEquipVM = equipsToModifDB.ListeEquipementsXZone(id.Value);
            string nomZone = pflEquipDB.NomZoneXEquipement(id.Value);

            EquipsXModifVM vm = new EquipsXModifVM()
            {
                ListeEquipements = listEquipVM,
                NomZone = nomZone,
                IdZone = id.Value
            };
            this.HttpContext.AddToSession("EquipsXModifVM", vm);

            return View("EquipsXModif", vm);
        }

        /// <summary>
        /// Rajouter une fiche materiel pour un equipement X
        /// </summary>
        /// <param name="id">id equipement</param>
        /// <returns></returns>
        public IActionResult Ajouterfiche(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
            vm.IdEquipement = id.Value;
            ViewBag.ModalAddingFic = "show";
            this.HttpContext.AddToSession("EquipsXModifVM", vm);
            return View("EquipsXModif", vm);
        }

        /// <summary>
        /// Post pour écrire la fiche dans la base des données
        /// </summary>
        /// <param name="file">contenu document</param>
        /// <param name="id">id equipement</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ValidAjoutFicheAsync(IFormFile file, int? id)
        {
            byte[] contenuDoc;
            bool isOk = false;
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");

            // Déterminer si le document est bien un .pdf
            string regexPatt = @"(.pdf)$"; // si le fichier contient à la fin de ligne un .pdf

            Regex Rg = new Regex(regexPatt);
            Match match = Rg.Match(file.FileName);
            if(!match.Success)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.MessagePopUp = "Seulement les fiches materiels en format PDF sont acceptées";
                ViewBag.ModalAddingFic = "show";
                return View("EquipsXModif", vm);
            }
            else
            {
                // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
                using (var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    contenuDoc = datastream.ToArray();
                }

                // Ajouter le document dans la BDD car il s'agit d'un pdf
                isOk = equipsToModifDB.AjouterFicheXEquipement(id.Value, contenuDoc , file.FileName);
                if (isOk)
                {
                    vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                    this.HttpContext.AddToSession("EquipsXModifVM", vm);
                    return View("EquipsXModif", vm);
                }
                else
                {
                    ModelState.AddModelError("", "Une erreur est survenue et la fiche materiel n'a pas pu être sauvegardée correctement");
                    vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                    this.HttpContext.AddToSession("EquipsXModifVM", vm);
                    return View("EquipsXModif", vm);
                }
            }     
        }

        /// <summary>
        /// Demande de suppression d'une fiche
        /// </summary>
        /// <param name="id">id fiche</param>
        /// <returns></returns>
        public IActionResult SupprimerFiche(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
            vm.IdDoc = id.Value;
            ViewBag.ModalConfirSupp = "show";
            this.HttpContext.AddToSession("EquipsXModifVM", vm);
            return View("EquipsXModif", vm);
        }

        [HttpPost]
        public IActionResult ConfSuppFiche(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");

            // supprimer la fiche de la BDD
            bool isOk = equipsToModifDB.SupprimerFicheMat(id.Value);
            if (!isOk)
            {
                ModelState.AddModelError("", "Une erreur est survenu lors de la suppression de la fiche materiel");
                vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                this.HttpContext.AddToSession("EquipsXModifVM", vm);
                return View("EquipsXModif", vm);
            }
            else
            {
                vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                this.HttpContext.AddToSession("EquipsXModifVM", vm);
                return View("EquipsXModif", vm);
            }
        }

        /// <summary>
        /// Remplacer une fiche materiel
        /// </summary>
        /// <param name="id">id fiche materiel</param>
        /// <returns></returns>
        public IActionResult ModifierFiche(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
            vm.IdFicheMat = id.Value;
            ViewBag.ModalModifFiche = "show";
            this.HttpContext.AddToSession("EquipsXModifVM", vm);
            return View("EquipsXModif", vm);
        }

        /// <summary>
        /// Action pour valider la modification d'une fiche materiel
        /// </summary>
        /// <param name="file">pdf</param>
        /// <param name="id">id fiche</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ValidModifFicheAsync(IFormFile file, int? id)
        {
            byte[] contenuDoc;
            bool isOk = false;
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");

            // Déterminer si le document est bien un .pdf
            string regexPatt = @"(.pdf)$"; // si le fichier contient à la fin de ligne un .pdf

            Regex Rg = new Regex(regexPatt);
            Match match = Rg.Match(file.FileName);
            if (!match.Success)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.MessagePopUp = "Seulement les fiches materiels en format PDF sont acceptées";
                ViewBag.ModalAddingFic = "show";
                return View("EquipsXModif", vm);
            }
            else
            {
                // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
                using (var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    contenuDoc = datastream.ToArray();
                }

                // Ajouter le document dans la BDD car il s'agit d'un pdf
                isOk = equipsToModifDB.ModifierFicheMat(id.Value, contenuDoc, file.FileName);
                if (isOk)
                {
                    vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                    this.HttpContext.AddToSession("EquipsXModifVM", vm);
                    return View("EquipsXModif", vm);
                }
                else
                {
                    ModelState.AddModelError("", "Une erreur est survenue et la fiche materiel n'a pas pu être sauvegardée correctement");
                    vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                    this.HttpContext.AddToSession("EquipsXModifVM", vm);
                    return View("EquipsXModif", vm);
                }
            }
        }

        /// <summary>
        /// Demande de suppression d'une fiche
        /// </summary>
        /// <param name="id">id fiche</param>
        /// <returns></returns>
        public IActionResult SupprimerEquip(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
            vm.IdEquipement = id.Value;
            ViewBag.ModalSuppEquip = "show";
            this.HttpContext.AddToSession("EquipsXModifVM", vm);
            return View("EquipsXModif", vm);
        }

        [HttpPost]
        public IActionResult ConfSuppEquip(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");

            // supprimer l'équipement en mettant la variable bool equip_delete à true
            bool isOk = equipsToModifDB.SupprimerEquipement(id.Value);
            if (!isOk)
            {
                ModelState.AddModelError("", "Une erreur est survenu lors de la suppression de l'équipement");
                vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                this.HttpContext.AddToSession("EquipsXModifVM", vm);
                return View("EquipsXModif", vm);
            }
            else
            {
                vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                this.HttpContext.AddToSession("EquipsXModifVM", vm);
                return View("EquipsXModif", vm);
            }
        }

        /*public IActionResult ModifNumGMAO(int? id)
        {
            // Récupérer la session "EquipsXModifVM"
            EquipsXModifVM vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
            vm.IdEquipement = id.Value;
            vm.NomEquipement = equipsToModifDB.ObtNomEquipement(id.Value);
            ViewBag.ModifNumGmao = "show";
            this.HttpContext.AddToSession("EquipsXModifVM", vm);
            return View("EquipsXModif", vm);
        }

        [HttpPost]
        public IActionResult ConfNumGMAO(EquipsXModifVM vm, int? id)
        {            
            if (ModelState.IsValid)
            {
                bool IsOk = equipsToModifDB.ModifierNumGMAO(vm.NumGmao, id.Value);
                if (!IsOk)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de la modification du numèro GMAO");
                    vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
                    return View("EquipsXModif", vm);
                }
                else
                {
                    vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
                    vm.ListeEquipements = equipsToModifDB.ListeEquipementsXZone(vm.IdZone);
                    this.HttpContext.AddToSession("EquipsXModifVM", vm);
                    return View("EquipsXModif", vm);
                }
            }
            else
            {
                vm = HttpContext.GetFromSession<EquipsXModifVM>("EquipsXModifVM");
                ViewBag.ModifNumGmao = "show";
                return View("EquipsXModif", vm);
            }
        }*/
    }
}
