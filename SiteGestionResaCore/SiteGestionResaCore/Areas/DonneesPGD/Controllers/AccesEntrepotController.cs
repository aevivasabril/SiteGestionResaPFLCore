using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.DonneesPGD.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Areas.DonneesPGD.Controllers
{
    [Area("DonneesPGD")]
    public class AccesEntrepotController : Controller
    {
        private readonly IEssaisXEntrepotDB entrepotDB;
        private readonly UserManager<utilisateur> userManager;

        public AccesEntrepotController(
            IEssaisXEntrepotDB entrepotDB,
            UserManager<utilisateur> userManager)
        {
            this.entrepotDB = entrepotDB;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CreationEntrepotAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            EssaisXEntrepotVM vm = new EssaisXEntrepotVM();
            vm.InfosResasSansEntrepot = entrepotDB.ObtenirResasSansEntrepotUsr(user);

            return View("ListeEssaisXEntrepot", vm);
        }

        /// <summary>
        /// Action pour afficher les infos sur le projet d'un essai sur la liste
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public IActionResult VoirInfosProj(int id)
        {
            InfosProjet vm = entrepotDB.ObtenirInfosProjet(id);
            return PartialView("~/Views/Shared/_DisplayInfosProjet.cshtml", vm);
        }

        /// <summary>
        /// Action pour afficher les infos sur un essai de la liste 
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChildVM model = entrepotDB.ObtenirInfosEssai(id);
            return PartialView("~/Views/Shared/_DisplayInfosEssai.cshtml", model);
        }

        /// <summary>
        /// Action pour afficher les réservations d'un essai sur la liste
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirReservations(int id)
        {
            EquipementsReservesVM vm = new EquipementsReservesVM()
            {
                Reservations = entrepotDB.InfosReservations(id)
            };
            return PartialView("~/Views/Shared/_DisplayEquipsReserves.cshtml", vm);
        }

        public IActionResult CreationEntrepotEssai(int? id)
        {
            // initialisation de session pour l'essai X
            CreationEntrepotVM vm = new CreationEntrepotVM()
            {
                ListReservationsXEssai = entrepotDB.ListeReservationsXEssai(id.Value),
                idEssai = id.Value,
                ListeTypeDoc = entrepotDB.ListeTypeDocuments(),
                ListDocsPartieUn = new List<DocAjoutePartieUn>(),
                ListDocsPartieDeux = new List<DocAjoutePartieDeux>()
            };

            this.HttpContext.AddToSession("CreationEntrepotVM", vm);

            return View("CreationEntrepotXEssai",vm );
        }

        public IActionResult OuvrirUploadDoc(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            CreationEntrepotVM vm = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
            //vm.OpenUploadPartUn = "";
            // Création d'une liste dropdownlist pour sélectionner le type de document d'une activité
            vm.TypeDocumentItem = entrepotDB.ListeTypeDocumentsXActivite(id.Value).Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_document+": " + f.identificateur });
            vm.NomActivite = entrepotDB.ObtenirNomActivite(id.Value);
            vm.IDActivite = entrepotDB.ObtenirIDActivite(id.Value);

            this.HttpContext.AddToSession("CreationEntrepotVM", vm);
            ViewBag.ModalDocOne = "show";
            return View("CreationEntrepotXEssai", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AjouterDocumentAsync(IFormFile file, string description, CreationEntrepotVM vm)
        {
            DocAjoutePartieUn docAjoutePartieUn = new DocAjoutePartieUn();

            if (ModelState.IsValid)
            {
                docAjoutePartieUn.NomDocument = file.FileName.ToString();
                docAjoutePartieUn.TypeActiviteID = vm.IDActivite;
                docAjoutePartieUn.TypeDonneesID = vm.TypeDocumentID;
                docAjoutePartieUn.TypeActivite = entrepotDB.ObtenirNomActivite(vm.IDActivite);
                docAjoutePartieUn.TypeDonnees = entrepotDB.ObtenirNomTypeDonnees(vm.TypeDocumentID);
                // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
                using (var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    docAjoutePartieUn.data = datastream.ToArray();
                }
                // Récupérer la session "CreationEntrepotVM"
                CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
                model.ListDocsPartieUn.Add(docAjoutePartieUn);

                this.HttpContext.AddToSession("CreationEntrepotVM", model); // Sauvegarder le model!

                return View("CreationEntrepotXEssai", model);
            }
            else
            {
                // Récupérer la session "CreationEntrepotVM"
                CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
                ViewBag.ModalDocOne = "show";
                return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
            }

        }

        public IActionResult SupprimerDocPartieUn(int id)
        {
            // Récupérer la session "CreationEntrepotVM"
            CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
            // Retirer le document de la liste
            model.ListDocsPartieUn.RemoveAt(id);

            this.HttpContext.AddToSession("CreationEntrepotVM", model); // Sauvegarder le model!

            return View("CreationEntrepotXEssai", model);

        }

        public IActionResult SupprimerDocPartieDeux(int id)
        {
            // Récupérer la session "CreationEntrepotVM"
            CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
            // Retirer le document de la liste
            model.ListDocsPartieDeux.RemoveAt(id);

            this.HttpContext.AddToSession("CreationEntrepotVM", model); // Sauvegarder le model!

            return View("CreationEntrepotXEssai", model);

        }

        public IActionResult OuvrirUploadDocTwo(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            CreationEntrepotVM vm = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");

            // Création d'une liste dropdownlist pour sélectionner le type de document pour un équipement
            vm.TypeDocumentItem = entrepotDB.ListeTypeDocumentsXEquip(id.Value).Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_document + ": " + f.identificateur });
            vm.NomEquipement = entrepotDB.ObtenirNomEquipement(id.Value);
            vm.IDEquipement = id.Value;
            vm.IDActivite = entrepotDB.ObtenirIdActiviteXequip(id.Value);

            this.HttpContext.AddToSession("CreationEntrepotVM", vm);
            ViewBag.ModalDocTwo = "show";
            return View("CreationEntrepotXEssai", vm);
        }


        [HttpPost]
        public async Task<IActionResult> AjouterDocumentXEquipAsync(IFormFile file, string description, CreationEntrepotVM vm)
        {
            DocAjoutePartieDeux docAjoutePartieDeux = new DocAjoutePartieDeux();

            if (ModelState.IsValid)
            {
                docAjoutePartieDeux.NomDocument = file.FileName.ToString();
                docAjoutePartieDeux.IdActivite = vm.IDActivite;
                docAjoutePartieDeux.IdTypeDonnees = vm.TypeDocumentID;
                docAjoutePartieDeux.TypeDonnees = entrepotDB.ObtenirNomTypeDonnees(vm.TypeDocumentID);
                docAjoutePartieDeux.NomActivite = entrepotDB.ObtenirNomActivite(vm.IDActivite);
                docAjoutePartieDeux.IdEquipement = vm.IDEquipement;
                docAjoutePartieDeux.NomEquipement = entrepotDB.ObtenirNomEquipement(vm.IDEquipement);
                docAjoutePartieDeux.TypeDonnees = entrepotDB.ObtenirNomTypeDonnees(vm.TypeDocumentID);
                // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
                using (var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    docAjoutePartieDeux.ContenuDoc = datastream.ToArray();
                }
                // Récupérer la session "CreationEntrepotVM"
                CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
                model.ListDocsPartieDeux.Add(docAjoutePartieDeux);

                this.HttpContext.AddToSession("CreationEntrepotVM", model); // Sauvegarder le model!

                return View("CreationEntrepotXEssai", model);
            }
            else
            {
                // Récupérer la session "CreationEntrepotVM"
                CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
                ViewBag.ModalDocTwo = "show";
                return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
            }
        }

        [HttpPost]
        public IActionResult ValiderCreationEntrepot()
        {
            bool IsDocStock = false;
            // Récupérer la session "CreationEntrepotVM"
            CreationEntrepotVM model = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");

            // Ecrire les documents du type UN dans la base de données
            IsDocStock = entrepotDB.EcrireDocTypeUn(model);
            if (IsDocStock)
            {
                IsDocStock = entrepotDB.EcrireDocTypeDeux(model);
                if (IsDocStock)
                {
                    // TODO: déclarer que l'essai a un entrepot des documents 
                    // TODO: generer les fichiers excel des données PcVue
                    // TODO: générer le pdf avec les infos essai
                    return View("ConfirmationEntrepot");
                }
                else
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème d'écriture des documents dans la base de données";
                    return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                }
            }
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème d'écriture des documents dans la base de données";
                return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
            }
        }
    }
}
