using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;
using System.Text;

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
            // Vérifier si le document est supprimable: document different à "Informations_Essai_##.txt" et "DonneesPcVue_..."
            bool IsDocSupp = accesEntrepotDB.DocSupprimable(id.Value);
            if(IsDocSupp == true)
            {
                bool IsDeletedOk = accesEntrepotDB.SupprimerDocument(id.Value);
                if (IsDeletedOk == false)
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
            }
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Vous ne pouvez pas supprimer ce document car il a été généré automatiquement lors de la création entrepôt!";
                vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(idEssai);
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
            }
            
            return View("MesEntrepots", vm);
;       }

        /// <summary>
        /// Télécharger tous les documents pour un projet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult TelechargerZip(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");
            // Créer un dossier temporaire dans mon disque C uniquement pour créer l'arborescence et génerer le .zip
            string path = @"C:\DonneesProjetPGD";

            #region Créer le directoire pour stocker l'arborescence

            if(accesEntrepotDB.CreateDirectoryTemp(path) != true)
            { 
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème pour créer le dossier: " + path;
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                goto ENDT;
            }
            #endregion

            // Obtenir le projet et les essais avec un entrepot crée
            var projet = accesEntrepotDB.GetProjet(id.Value);
            var essais = accesEntrepotDB.ListEssaiEntrepotxProjet(id.Value);

            string dossNameProj = "projet-" + projet.titre_projet + "-Num-" + projet.num_projet; // Supprimer les espaces et supprimer les accents!

            // supprimer les accents d'un string
            byte[] bytesP = System.Text.Encoding.GetEncoding(1251).GetBytes(dossNameProj);
            dossNameProj = System.Text.Encoding.ASCII.GetString(bytesP);

            //remplacer les espaces ou caracteres especiaux par des -
            string NameDossierProj = accesEntrepotDB.CorrigerStringNomDossier(dossNameProj); // limiter les noms des dossiers car sinon trop longs
            // Créer un sous dossier avec le nom du projet 
            string pathP = path + @"\" + NameDossierProj;

            #region Créer le directoire "projet"

            if (accesEntrepotDB.CreateDirectoryTemp(pathP) != true)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème pour créer le dossier: " + pathP;
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                goto ENDT;
            }
            #endregion

            // Pour chaque essai créer un dossier et ajouter tous les documents associés à cet essai dans le dossier
            foreach (var essai in essais)
            {
                string dossNameEssai = essai.titreEssai + "-Num-" + essai.id; // Supprimer les espaces et supprimer les accents!

                #region supprimer les accents d'un string

                byte[] bytesE = System.Text.Encoding.GetEncoding(1251).GetBytes(dossNameEssai);
                dossNameEssai = System.Text.Encoding.ASCII.GetString(bytesE);

                #endregion

                #region remplacer les espaces ou caracteres especiaux par des -

                string NameDossier = accesEntrepotDB.CorrigerStringNomDossier(dossNameEssai);

                #endregion
                string pathE = pathP + @"\" + NameDossier;

                #region Créer le directoire "essai"

                if (accesEntrepotDB.CreateDirectoryTemp(pathE) != true)
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème pour créer le dossier: " + pathE;
                    this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                    goto ENDT;
                }
                #endregion

                var ListDocs = accesEntrepotDB.ListDocsEssai(essai.id);
                foreach(var doc in ListDocs)
                {
                    // Recréer le fichier et l'ajouter dans le dossier "essai"
                    System.IO.File.WriteAllBytes(pathE + @"\" + doc.nom_document, doc.contenu_document);
                }

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(pathP);
                    
                    string zipName = String.Format(NameDossierProj+".zip");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {                       
                        zip.Save(memoryStream);
                        Directory.Delete(path, true); // Supprimer le dossier créé en local! 
                        return File(memoryStream.ToArray(), "application/zip", zipName);
                    }
                }         
            } 
            ENDT:
            return View("MesEntrepots", vm);
        }
    }
}
