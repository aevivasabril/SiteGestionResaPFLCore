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
using Microsoft.AspNetCore.Authorization;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Areas.DonneesPGD.Controllers
{
    [Area("DonneesPGD")]
    //[Authorize(Roles = "Admin, MainAdmin")]
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
            vm.TotalKo = accesEntrepotDB.CalculTotalKoEntrepot(id.Value);
            vm.IdProjSelect = id.Value;
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
            vm.TotalKo = accesEntrepotDB.CalculTotalKoEntrepot(vm.IdProjSelect);
            this.HttpContext.AddToSession("MesEntrepotsVM", vm);
            return View("MesEntrepots", vm);
        }

        public IActionResult SupprimerDoc(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");
            int idEssai = accesEntrepotDB.RecupIdEssaiXDoc(id.Value);
            // Vérifier si le document est supprimable: document different à "Informations_Essai_##.txt" et "DonneesPcVue_..."
            //bool IsDocSupp = accesEntrepotDB.DocSupprimable(id.Value);
            //if(IsDocSupp == true)
            //{
                bool IsDeletedOk = accesEntrepotDB.SupprimerDocument(id.Value);
                if (IsDeletedOk == false)
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème de suppression des documents entrepôt, essayez ultérieurement";
                }
                else
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Document supprimé!";
                    vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(idEssai);
                    this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                }
            /*}
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Vous ne pouvez pas supprimer ce document car il a été généré automatiquement lors de la création entrepôt!";
                vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(idEssai);
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
            }*/
            
            return View("MesEntrepots", vm);
        }

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
            // Obtenir le projet et les essais avec un entrepot crée
            projet projet = accesEntrepotDB.GetProjet(id.Value);
            List<essai> essais = accesEntrepotDB.ListEssaiEntrepotxProjet(id.Value);
            string titreProj = "";
            string titreEssai = "";
            string nomEquip = "";
            string typeDoc = "";

            #region Créer le directoire pour stocker l'arborescence

            if (accesEntrepotDB.CreateDirectoryTemp(path) != true)
            { 
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème pour créer le dossier: " + path;
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                goto ENDT;
            }
            #endregion

            // Limiter la quantité des caracteres 
            titreProj = accesEntrepotDB.TraiterChaineCaract(projet.titre_projet, 15);
            string dossNameProj = projet.num_projet + "-" + titreProj; // Supprimer les espaces et supprimer les accents!

            // Créer un sous dossier avec le nom du projet 
            string pathP = path + @"\" + dossNameProj;

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
                titreEssai = accesEntrepotDB.TraiterChaineCaract(essai.titreEssai, 15);
               
                string dossNameEssai = titreEssai + "-N-" + essai.id; 

                string pathE = pathP + @"\" + dossNameEssai;

                #region Créer le directoire "essai"

                if (accesEntrepotDB.CreateDirectoryTemp(pathE) != true)
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème pour créer le dossier: " + pathE;
                    this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                    goto ENDT;
                }
                #endregion

                var ListDocs = accesEntrepotDB.ListDocsEssai(essai.id).GroupBy(d=>d.type_activiteID);
                foreach(var doc in ListDocs)
                {
                    // Récupérer le nom de l'id Activité
                    var docu = accesEntrepotDB.ObtActivite(doc.Key);
                    string nameAct = accesEntrepotDB.TraiterChaineCaract(docu.id.ToString() + "_" + docu.nom_activite, 16);     

                    string PathAct = pathE + @"\" + nameAct;

                    if (accesEntrepotDB.CreateDirectoryTemp(PathAct) != true)
                    {
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Problème pour créer le dossier: " + PathAct;
                        this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                        goto ENDT;
                    }

                    foreach (var d in doc)
                    {
                        string PathDoc = PathAct;
                        string PathTypeDoc = "";
                        // Voir si il y a des documents liés à un équipement
                        if(d.equipementID != null)
                        {
                            var equip = accesEntrepotDB.GetEquipement(d.equipementID.Value);
                            nomEquip = accesEntrepotDB.TraiterChaineCaract(equip.numGmao + "-" + equip.nom, 16);
                            
                            PathDoc = PathDoc + @"\" + nomEquip;
                            if (accesEntrepotDB.CreateDirectoryTemp(PathDoc) != true)
                            {
                                ViewBag.AfficherMessage = true;
                                ViewBag.Message = "Problème pour créer le dossier: " + PathDoc;
                                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                                goto ENDT;
                            }
                        }
                        // Determiner le type de document pour créer le dossier
                        type_document type = accesEntrepotDB.ObtenirTypeDocument(d.type_documentID);

                        typeDoc = accesEntrepotDB.TraiterChaineCaract(type.nom_document, 10);
                        PathTypeDoc = PathDoc + @"\" + typeDoc;
                        if (accesEntrepotDB.CreateDirectoryTemp(PathTypeDoc) != true)
                        {
                            ViewBag.AfficherMessage = true;
                            ViewBag.Message = "Problème pour créer le dossier: " + PathDoc;
                            this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                            goto ENDT;
                        }

                        #region Supprimer les accents d'un string

                        byte[] bytesP = System.Text.Encoding.GetEncoding(1251).GetBytes(d.nom_document);
                        var nomDoc = System.Text.Encoding.ASCII.GetString(bytesP);
                        string nomDocSimpl = accesEntrepotDB.TraiterChaineCaract(nomDoc, 15);
                        #endregion

                        // Recréer le fichier et l'ajouter dans le dossier "activite"
                        System.IO.File.WriteAllBytes(PathTypeDoc + @"\" + nomDocSimpl, d.contenu_document);
                    }
                }                     
            }

            #region Création du dossier .zip à retourner pour téléchargement
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(pathP);

                string zipName = String.Format(dossNameProj + ".zip");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    Directory.Delete(path, true); // Supprimer le dossier créé en local! 
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }
            #endregion

        ENDT:
            return View("MesEntrepots", vm);
        }

        public IActionResult SupprimerEntrepot(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");

            vm.NumProjetSelect = accesEntrepotDB.ObtNumProjet(id.Value);
            vm.IdProjSelect = id.Value;
            vm.HideListeDocsXEssai = "none";
            vm.HideListeDocs = "none";

            ViewBag.modalSuppEnt = "show";
            return View("MesEntrepots", vm);
        }

        /// <summary>
        /// Action pour supprimer tous les documents stockés pour un projet (N essais = N entrepôts)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmSuppEntrepotAsync(int id)
        {
            bool isOk = false;

            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("MesEntrepotsVM");

            isOk = accesEntrepotDB.SupprimerEntrepotXProjet(id);
            if (isOk)
            {
                vm.ListEntrepotXProjet = accesEntrepotDB.ObtenirListProjetsAvecEntrepotCree(user);
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Entrepôt supprimé avec succès";
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                return View("MesEntrepots", vm);
            }
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Un problème est survenu lors de la suppression de l'entrepôt, réessayez ultérieurement";
                return View("MesEntrepots", vm);
            }
        }

        [Authorize(Roles = "DonneesAdmin, MainAdmin")]
        public IActionResult AccesTousEntrepots()
        {
            // Obtenir les infos de l'utilisateur authentifié
            //var user = await userManager.FindByIdAsync(User.GetUserId());
            MesEntrepotsVM entrepotsVM = new MesEntrepotsVM();
            entrepotsVM.ListEntrepotXProjet = accesEntrepotDB.ObtenirListTousEntrepots();
            entrepotsVM.HideListeDocsXEssai = "none";
            entrepotsVM.HideListeDocs = "none";
            entrepotsVM.ListEssaiAvecEntrepot = new List<EssaiAvecEntrepotxProj>();
            entrepotsVM.ListDocsXEssai = new List<DocXEssai>();
            this.HttpContext.AddToSession("TousEntrepotsVM", entrepotsVM);

            return View("TousEntrepots", entrepotsVM);
        }

        public IActionResult EssaisXProjetAdm(int? id)
        {
            // Récupérer la session "TousEntrepots"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("TousEntrepotsVM");
            vm.ListEssaiAvecEntrepot = accesEntrepotDB.ObtenirListEssaiAvecEntrepot(id.Value);
            vm.HideListeDocsXEssai = "";
            vm.HideListeDocs = "none";
            vm.NumProjetSelect = accesEntrepotDB.ObtNumProjet(id.Value);
            vm.TotalKo = accesEntrepotDB.CalculTotalKoEntrepot(id.Value);
            vm.IdProjSelect = id.Value;
            this.HttpContext.AddToSession("TousEntrepotsVM", vm);

            return View("TousEntrepots", vm);
        }

        /// <summary>
        /// Télécharger tous les documents pour un projet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult TelechargerZipAdm(int? id)
        {
            // Récupérer la session "TousEntrepots"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("TousEntrepotsVM");
            // Créer un dossier temporaire dans mon disque C uniquement pour créer l'arborescence et génerer le .zip
            string path = @"C:\DonneesProjetPGD";
            // Obtenir le projet et les essais avec un entrepot crée
            projet projet = accesEntrepotDB.GetProjet(id.Value);
            List<essai> essais = accesEntrepotDB.ListEssaiEntrepotxProjet(id.Value);
            string titreProj = "";
            string titreEssai = "";
            string nomEquip = "";
            string typeDoc = "";

            #region Créer le directoire pour stocker l'arborescence

            if (accesEntrepotDB.CreateDirectoryTemp(path) != true)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème pour créer le dossier: " + path;
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
                goto ENDT;
            }
            #endregion

            // Limiter la quantité des caracteres 
            titreProj = accesEntrepotDB.TraiterChaineCaract(projet.titre_projet, 30);
            string dossNameProj = "projet-" + titreProj + "-Num-" + projet.num_projet; // Supprimer les espaces et supprimer les accents!

            // Créer un sous dossier avec le nom du projet 
            string pathP = path + @"\" + dossNameProj;

            #region Créer le directoire "projet"

            if (accesEntrepotDB.CreateDirectoryTemp(pathP) != true)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème pour créer le dossier: " + pathP;
                this.HttpContext.AddToSession("TousEntrepotsVM", vm);
                goto ENDT;
            }
            #endregion

            // Pour chaque essai créer un dossier et ajouter tous les documents associés à cet essai dans le dossier
            foreach (var essai in essais)
            {
                titreEssai = accesEntrepotDB.TraiterChaineCaract(essai.titreEssai, 30);

                string dossNameEssai = titreEssai + "-Num-" + essai.id;

                string pathE = pathP + @"\" + dossNameEssai;

                #region Créer le directoire "essai"

                if (accesEntrepotDB.CreateDirectoryTemp(pathE) != true)
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème pour créer le dossier: " + pathE;
                    this.HttpContext.AddToSession("TousEntrepotsVM", vm);
                    goto ENDT;
                }
                #endregion

                var ListDocs = accesEntrepotDB.ListDocsEssai(essai.id).GroupBy(d => d.type_activiteID);
                foreach (var doc in ListDocs)
                {
                    // Récupérer le nom de l'id Activité
                    var docu = accesEntrepotDB.ObtActivite(doc.Key);
                    string nameAct = accesEntrepotDB.TraiterChaineCaract(docu.id.ToString() + "_" + docu.nom_activite, 25);

                    string PathAct = pathE + @"\" + nameAct;

                    if (accesEntrepotDB.CreateDirectoryTemp(PathAct) != true)
                    {
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Problème pour créer le dossier: " + PathAct;
                        this.HttpContext.AddToSession("TousEntrepotsVM", vm);
                        goto ENDT;
                    }

                    foreach (var d in doc)
                    {
                        string PathDoc = PathAct;
                        string PathTypeDoc = "";
                        // Voir si il y a des documents liés à un équipement
                        if (d.equipementID != null)
                        {
                            var equip = accesEntrepotDB.GetEquipement(d.equipementID.Value);
                            nomEquip = accesEntrepotDB.TraiterChaineCaract(equip.nom, 31);

                            PathDoc = PathDoc + @"\" + nomEquip;
                            if (accesEntrepotDB.CreateDirectoryTemp(PathDoc) != true)
                            {
                                ViewBag.AfficherMessage = true;
                                ViewBag.Message = "Problème pour créer le dossier: " + PathDoc;
                                this.HttpContext.AddToSession("TousEntrepotsVM", vm);
                                goto ENDT;
                            }
                        }
                        // Determiner le type de document pour créer le dossier
                        type_document type = accesEntrepotDB.ObtenirTypeDocument(d.type_documentID);

                        typeDoc = accesEntrepotDB.TraiterChaineCaract(type.nom_document, 25);
                        PathTypeDoc = PathDoc + @"\" + typeDoc;
                        if (accesEntrepotDB.CreateDirectoryTemp(PathTypeDoc) != true)
                        {
                            ViewBag.AfficherMessage = true;
                            ViewBag.Message = "Problème pour créer le dossier: " + PathDoc;
                            this.HttpContext.AddToSession("TousEntrepotsVM", vm);
                            goto ENDT;
                        }

                        #region Supprimer les accents d'un string

                        byte[] bytesP = System.Text.Encoding.GetEncoding(1251).GetBytes(d.nom_document);
                        var nomDoc = System.Text.Encoding.ASCII.GetString(bytesP);
                        #endregion

                        // Recréer le fichier et l'ajouter dans le dossier "activite"
                        System.IO.File.WriteAllBytes(PathTypeDoc + @"\" + nomDoc, d.contenu_document);
                    }
                }
            }

            #region Création du dossier .zip à retourner pour téléchargement
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(pathP);

                string zipName = String.Format(dossNameProj + ".zip");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    Directory.Delete(path, true); // Supprimer le dossier créé en local! 
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }
        #endregion

        ENDT:
            return View("TousEntrepots", vm);
        }

        public IActionResult SupprimerEntrepotAdm(int? id)
        {
            // Récupérer la session "TousEntrepotsVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("TousEntrepotsVM");

            vm.NumProjetSelect = accesEntrepotDB.ObtNumProjet(id.Value);
            vm.IdProjSelect = id.Value;
            vm.HideListeDocsXEssai = "none";
            vm.HideListeDocs = "none";

            ViewBag.modalSuppEnt = "show";
            return View("TousEntrepots", vm);
        }

        /// <summary>
        /// Action pour supprimer tous les documents stockés pour un projet (N essais = N entrepôts)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmSuppEntrepotAdmAsync(int id)
        {
            bool isOk = false;

            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("TousEntrepotsVM");

            isOk = accesEntrepotDB.SupprimerEntrepotXProjet(id);
            if (isOk)
            {
                vm.ListEntrepotXProjet = accesEntrepotDB.ObtenirListProjetsAvecEntrepotCree(user);
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Entrepôt supprimé avec succès";
                this.HttpContext.AddToSession("TousEntrepotsVM", vm);
                return View("TousEntrepots", vm);
            }
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Un problème est survenu lors de la suppression de l'entrepôt, réessayez ultérieurement";
                return View("TousEntrepots", vm);
            }
        }

        public IActionResult DocsXEssaiAdm(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("TousEntrepotsVM");
            vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(id.Value);
            vm.HideListeDocs = "";
            vm.HideListeDocsXEssai = "";
            vm.HideListeDocs = "";
            vm.IdEssai = id.Value;
            vm.TitreEssai = accesEntrepotDB.ObtTitreEssai(id.Value);
            vm.TotalKo = accesEntrepotDB.CalculTotalKoEntrepot(vm.IdProjSelect);
            this.HttpContext.AddToSession("TousEntrepotsVM", vm);
            return View("TousEntrepots", vm);
        }

        public IActionResult SupprimerDocAdm(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            MesEntrepotsVM vm = HttpContext.GetFromSession<MesEntrepotsVM>("TousEntrepotsVM");
            int idEssai = accesEntrepotDB.RecupIdEssaiXDoc(id.Value);
            // Vérifier si le document est supprimable: document different à "Informations_Essai_##.txt" et "DonneesPcVue_..."
            //bool IsDocSupp = accesEntrepotDB.DocSupprimable(id.Value);
            //if(IsDocSupp == true)
            //{
            bool IsDeletedOk = accesEntrepotDB.SupprimerDocument(id.Value);
            if (IsDeletedOk == false)
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Problème de suppression des documents entrepôt, essayez ultérieurement";
            }
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Document supprimé!";
                vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(idEssai);
                this.HttpContext.AddToSession("TousEntrepotsVM", vm);
            }
            /*}
            else
            {
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Vous ne pouvez pas supprimer ce document car il a été généré automatiquement lors de la création entrepôt!";
                vm.ListDocsXEssai = accesEntrepotDB.ObtListDocsXEssai(idEssai);
                this.HttpContext.AddToSession("MesEntrepotsVM", vm);
            }*/

            return View("TousEntrepots", vm);
        }

        public IActionResult VoirInfosProjet(int id)
        {
            InfosProjet vm = accesEntrepotDB.ObtenirInfosProjet(id);
            return PartialView("~/Views/Shared/_DisplayInfosProjet.cshtml", vm);
        }

        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChildVM model = accesEntrepotDB.ObtenirInfosEssai(id);
            return PartialView("~/Views/Shared/_DisplayInfosEssai.cshtml", model);
        }
    }
}
