using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Controllers
{
    [Area("Equipe")]
    public class ModifDocAqController : Controller
    {
        private readonly IModifAqDB modifAqDB;

        public ModifDocAqController(
            IModifAqDB modifAqDB)
        {
            this.modifAqDB = modifAqDB;
        }

        /// <summary>
        /// Vue principale avec l'affichage de la liste des documents 
        /// et le lien pour rajouter des nouvelles rubriques = documents
        /// </summary>
        /// <returns></returns>
        public IActionResult IndexModifDocsAQ()
        {
            DocAqModifVM vm = new DocAqModifVM();
            vm.ListDocToModif = modifAqDB.ObtenirDocsAQXModif();
            this.HttpContext.AddToSession("DocAqModifVM", vm);

            return View("VueModifDocsAQ", vm);
        }

        public IActionResult AjouterDocAq()
        {
            DocAqModifVM vm = HttpContext.GetFromSession<DocAqModifVM>("DocAqModifVM");
            ViewBag.ModalAddingDoc = "show";
            return View("VueModifDocsAQ", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ValidAjoutDocAsync(IFormFile file, string description, DocAqModifVM vm)
        {
            DocQualiteToModif docToAdd = new DocQualiteToModif();
            // Récupérer la session "DocAqModifVM"
            DocAqModifVM model = HttpContext.GetFromSession<DocAqModifVM>("DocAqModifVM");

            if (ModelState.IsValid)
            {
                docToAdd.NomDocument = file.FileName.ToString();
                docToAdd.DescripDoc = description;
                docToAdd.NomRubriqDoc = vm.NomRubriqueDoc;
                docToAdd.DernierDateModif = DateTime.Now;
               
                // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
                using (var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    docToAdd.ContenuDoc = datastream.ToArray();
                }

                bool isOK = modifAqDB.AjouterDocToBDD(docToAdd);
                if(isOK == false)
                {
                    ModelState.AddModelError("", "Problème pour enregistrer le nouveau document dans la BDD");
                    //ViewBag.ModalAddingDoc = "show";
                    return View("VueModifDocsAQ", model);
                }
                // Rafraichir la liste des documents
                model.ListDocToModif = modifAqDB.ObtenirDocsAQXModif();
                this.HttpContext.AddToSession("DocAqModifVM", model);
                return View("VueModifDocsAQ", model);
            }
            else
            {
                ViewBag.ModalAddingDoc = "show";
                return View("VueModifDocsAQ", model);
            }
            //return View("VueModifDocsAQ", vm);
        }

        public IActionResult SupprimerDoc(int? id)
        {
            DocAqModifVM model = HttpContext.GetFromSession<DocAqModifVM>("DocAqModifVM");
            model.IdDocToSupp = id.Value;
            model.NomRubriqueDoc = modifAqDB.ObtenirDocQualite(id.Value).nom_rubrique_doc;
            ViewBag.ModalSuppDoc = "show";
            return View("VueModifDocsAQ", model);
        }

        [HttpPost]
        public IActionResult ConfirmSuppDoc(int? id)
        {
            DocAqModifVM model = HttpContext.GetFromSession<DocAqModifVM>("DocAqModifVM");
            bool isOk = modifAqDB.SupprimerDocAQ(id.Value);
            if(!isOk)
            {
                ModelState.AddModelError("", "Problème lors de la suppression du document AQ");
                return View("VueModifDocsAQ", model);
            }
            else
            {
                model.ListDocToModif = modifAqDB.ObtenirDocsAQXModif();
                this.HttpContext.AddToSession("DocAqModifVM", model);
                return View("VueModifDocsAQ", model);
            }
        }
    }
}
