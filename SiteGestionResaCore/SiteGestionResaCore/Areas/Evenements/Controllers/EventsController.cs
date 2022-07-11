using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Evenements.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Evenements.Controllers
{
    [Area("Evenements")]
    public class EventsController : Controller
    {
        private readonly IEvenementDB evenementDB;

        public EventsController(
            IEvenementDB evenementDB)
        {           
            this.evenementDB = evenementDB;
        }

        public IActionResult AccueilEvenements()
        {
            EvenementsViewModel vm = new EvenementsViewModel();
            vm.ListEvenements = evenementDB.ListEvenements();
            this.HttpContext.AddToSession("EvenementsViewModel", vm);
            return View("GestionEvenements", vm);
        }

        [HttpPost]
        public IActionResult AjoutMessage(EvenementsViewModel vm)
        {
            EvenementsViewModel model = HttpContext.GetFromSession<EvenementsViewModel>("EvenementsViewModel");
            // Vérifier qu'il y a un message écrit
            if (vm.MessageXAjout == null)
            {
                ModelState.AddModelError(model.MessageXAjout, "Veuillez rajouter un message!");
                return View("GestionEvenements", model);
            }
            else
            {
                // Ajouter evenement ou message
                bool isOk = evenementDB.AjoutEvent(vm.MessageXAjout);
                if(isOk == false)
                {
                    ModelState.AddModelError("", "Problème lors de l'écriture du message, réessayez ulterieurement");
                    return View("GestionEvenements", model);
                }
                else
                {
                    model.ListEvenements = evenementDB.ListEvenements();
                    return View("GestionEvenements", model);
                }
            }
        }

        public IActionResult SupprimerMessage(int? id)
        {
            EvenementsViewModel model = HttpContext.GetFromSession<EvenementsViewModel>("EvenementsViewModel");
            // Ajouter evenement ou message
            bool isOk = evenementDB.SupprimerEvenement(id.Value);
            if (isOk == false)
            {
                ModelState.AddModelError("", "Problème lors de la suppression du message, réessayez ulterieurement");
                return View("GestionEvenements", model);
            }
            else
            {
                model.ListEvenements = evenementDB.ListEvenements();
                return View("GestionEvenements", model);
            }
        }
    }

}
