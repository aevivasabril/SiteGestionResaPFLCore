using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Evenements.Data;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEvenementDB evenementDB;

        public HomeController(
            IEvenementDB evenementDB)
        {
            this.evenementDB = evenementDB;
        }
        public ActionResult Index()
        {
            // Obtenir la liste des evenements
            HomeVM vm = new HomeVM();
            vm.Evenements = evenementDB.ListEvenements();
            return View("Index", vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Equipe()
        {
            return View();
        }
    }
}