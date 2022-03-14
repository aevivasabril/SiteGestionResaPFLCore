using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Controllers
{
    [Area("DonneesPGD")]
    public class AccesEntrepotController : Controller
    {
        private readonly IAccesEntrepotDB accesEntrepotDB;
        private readonly UserManager<utilisateur> userManager;

        public AccesEntrepotController(
            IAccesEntrepotDB accesEntrepotDB,
            UserManager<utilisateur> userManager)
        {
            this.accesEntrepotDB = accesEntrepotDB;
            this.userManager = userManager;
        }

        public async Task<IActionResult> MesEntrepotsAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());
            MesEntrepotsVM entrepotsVM = new MesEntrepotsVM();
            entrepotsVM.ListEntrepotXProjet = accesEntrepotDB.ObtenirListProjetsAvecEntrepotCree(user);
            entrepotsVM.HideListeDocsXEssai = "none";
            entrepotsVM.HideListeDocs = "none";
            entrepotsVM.ListEssaiAvecEntrepot = new List<EssaiAvecEntrepotxProj>();
            entrepotsVM.ListDocsXEssai = new List<DocXEssai>();
            this.HttpContext.AddToSession("MesEntrepotsVM", entrepotsVM);

            return View("MesEntrepots", entrepotsVM);
        }

        public IActionResult EssaisXProjet(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");
            vm.ListEssaiAvecEntrepot = accesEntrepotDB.ObtenirListEssaiAvecEntrepot(id.Value);
            vm.HideListeDocsXEssai = "";
            vm.HideListeDocs = "none";
            vm.NumProjetSelect = accesEntrepotDB.ObtNumProjet(id.Value);
            this.HttpContext.AddToSession("MesEntrepotsVM", vm);

            return View("MesEntrepots", vm);
        }

        public IActionResult DocsXEssai(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");
            vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(id.Value);
            vm.HideListeDocs = "";
            vm.HideListeDocsXEssai = "";
            vm.HideListeDocs = "";
            vm.IdEssai = id.Value;
            vm.TitreEssai = accesEntrepotDB.ObtTitreEssai(id.Value);
            this.HttpContext.AddToSession("MesEntrepotsVM", vm);
            return View("MesEntrepots", vm);
        }

        public IActionResult SupprimerDoc(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");
            int idEssai = accesEntrepotDB.RecupIdEssaiXDoc(id.Value);
            bool IsDeletedOk = accesEntrepotDB.SupprimerDocument(id.Value);
            if(IsDeletedOk == false)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème de suppression du document, essayez à nouveau";
            }
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Document supprimé!";
                vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(idEssai);
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
            }
            return View("MesEntrepots", vm);
;        }
    }
}
