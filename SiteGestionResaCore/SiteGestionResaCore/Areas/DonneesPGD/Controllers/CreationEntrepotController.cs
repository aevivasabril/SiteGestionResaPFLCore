using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.DonneesPGD.Data;
using SiteGestionResaCore.Areas.User.Data.DataPcVue;
using SiteGestionResaCore.Areas.User.Data.DonneesUser;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Areas.DonneesPGD.Controllers
{
    [Area("DonneesPGD")]
    public class CreationEntrepotController : Controller
    {
        private readonly IEssaisXEntrepotDB entrepotDB;
        private readonly IDonneesUsrDB donneesUserDB;
        private readonly UserManager<utilisateur> userManager;

        public CreationEntrepotController(
            IEssaisXEntrepotDB entrepotDB,
            IDonneesUsrDB donneesUserDB,
            UserManager<utilisateur> userManager)
        {
            this.entrepotDB = entrepotDB;
            this.donneesUserDB = donneesUserDB;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CreerEntrepotAsync()
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
            // Initialisation de session pour l'essai X
            CreationEntrepotVM vm = new CreationEntrepotVM()
            {
                ListReservationsXEssai = entrepotDB.ListeReservationsXEssai(id.Value),
                idEssai = id.Value,
                TitreEssai = entrepotDB.ObtenirTitreEssai(id.Value),
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id reservation</param>
        /// <returns></returns>
        public IActionResult OuvrirUploadDocTwo(int? id)
        {
            // Récupérer la session "CreationEntrepotVM"
            CreationEntrepotVM vm = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");

            var resa = entrepotDB.ObtenirResa(id.Value);
            // Création d'une liste dropdownlist pour sélectionner le type de document pour un équipement
            vm.TypeDocumentItem = entrepotDB.ListeTypeDocumentsXEquip(resa.equipementID).Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_document + ": " + f.identificateur });
            
            vm.IDEquipement = resa.equipementID;
            vm.NomEquipement = entrepotDB.ObtenirEquipement(resa.equipementID).nom;

            vm.idResa = id.Value;
            vm.IDActivite = entrepotDB.ObtenirIdActiviteXequip(resa.equipementID);

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
                docAjoutePartieDeux.NomEquipement = entrepotDB.ObtenirEquipement(vm.IDEquipement).nom;
                docAjoutePartieDeux.TypeDonnees = entrepotDB.ObtenirNomTypeDonnees(vm.TypeDocumentID);
                docAjoutePartieDeux.idReservation = vm.idResa;
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
                   
            var essai = entrepotDB.ObtenirEssai(model.idEssai);

            // Enregistrer la date de creation d'un entrepot pour un essai
            projet proj = entrepotDB.ObtenirProjetXEssai(essai.projetID);
                        
            if (essai.entrepot_cree == true) // Ajout des documents dans un entrepot déjà crée
            {
                // Ecrire les documents du type UN dans la base de données
                IsDocStock = entrepotDB.EcrireDocTypeUn(model);
                if (IsDocStock)
                {
                    IsDocStock = entrepotDB.EcrireDocTypeDeux(model);
                    if (IsDocStock)
                    {
                        //ViewBag.Message = "Document(s) ajouté(s) dans l'entrepôt essai N°" + model.idEssai;
                        return RedirectToAction("DocsXEssai", "AccesEntrepot", new { area ="DonneesPGD", id=model.idEssai });
                    }
                    else
                    {
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Problème d'écriture des documents ajoutés";
                        return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                    }
                }
                else
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème d'écriture des documents ajoutés";
                    return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                }
            }
            else  // Création d'entrepôt 
            {
                // Ecrire les documents du type UN dans la base de données
                IsDocStock = entrepotDB.EcrireDocTypeUn(model);
                if (IsDocStock)
                {
                    IsDocStock = entrepotDB.EcrireDocTypeDeux(model);
                    if (IsDocStock)
                    {
                        // TODO: generer les fichiers excel des données PcVue
                        #region récupérer les fichiers excel des équipements sous supervision
                        // vérifier si l'équipement est sous supervision des données (déjà fait pour la liste des réservations)
                        foreach (var resa in model.ListReservationsXEssai)
                        {
                            if (resa.FichierPcVue == "Données disponibles") // Si des données dispos alors créer l'excel
                            {
                                AllDataPcVue Donnees = donneesUserDB.ObtenirDonneesPcVue(resa.idResa);
                                StringBuilder csv = new StringBuilder();
                                string titreCsv = null;

                                #region  Créer un excel avec les données

                                // Déterminer les headers tableau
                                var headers = Donnees.DataEquipement.Select(d => d.NomCapteur).Distinct().ToList();
                                // Ajouter la colonne de date 
                                csv.Append("Date");
                                csv.Append(";");
                                csv.Append("Heure");

                                foreach (var dc in headers)
                                {
                                    csv.Append(";");
                                    csv.Append(dc);
                                }
                                csv.AppendLine();

                                // Reagrouper les données par date pour identifier chaque future ligne tableau
                                var reg = Donnees.DataEquipement.GroupBy(d => d.Chrono);
                                if (reg.Count() != 0)
                                {
                                    foreach (var group in reg)
                                    {
                                        csv.Append(group.Key.ToShortDateString());
                                        csv.Append(";");
                                        csv.Append(group.Key.ToShortTimeString());
                                        foreach (var r in group)
                                        {
                                            csv.Append(";");
                                            csv.Append(r.Value);
                                        }
                                        csv.AppendLine();
                                    }

                                    titreCsv = "DonneesProjet_EssaiId" + model.idEssai + "_" + essai.titreEssai + "_" + Donnees.NomEquipement + ".csv";

                                    var donneesPcVue = File(new System.Text.UTF8Encoding().GetBytes(csv.ToString()), "text/csv", titreCsv);
                                    // Obtenir l'équipement
                                    equipement equipem = entrepotDB.ObtenirEquipement(resa.IdEquipement);
                                    doc_essai_pgd doc = new doc_essai_pgd
                                    {
                                        contenu_document = donneesPcVue.FileContents,
                                        nom_document = donneesPcVue.FileDownloadName,
                                        date_creation = DateTime.Now,
                                        equipementID = resa.IdEquipement,
                                        essaiID = model.idEssai,
                                        type_activiteID = equipem.activiteID.Value,
                                        type_documentID = 4 // car c'est un tableau excel PcVue
                                    };

                                    IsDocStock = entrepotDB.SaveDocEssaiPgd(doc, "Excel PcVue");
                                    if (!IsDocStock)
                                    {
                                        ViewBag.AfficherMessage = true;
                                        ViewBag.Message = "Problème d'écriture dans la base de données d'un des fichiers excel PcVue";
                                        return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                        // TODO: générer le .txt avec les infos essai
                        #region générer un fichier .txt avec les informations "essai"
                        // récupérer projet auquel appartient l'essai
                        //projet proj = entrepotDB.ObtenirProjetXEssai(essai.projetID);
                        organisme orgProj = entrepotDB.ObtenirOrgXProj(proj.organismeID.Value);

                        StringBuilder txt = new StringBuilder();
                        string titreTxt = "Informations_Essai" + "_" + essai.id + ".txt";

                        #region ecriture dans le fichier
                        txt.Append("-------INFOS PROJET-------");
                        txt.AppendLine();
                        txt.Append("Titre Projet  : ");
                        txt.Append(proj.titre_projet);
                        txt.AppendLine();
                        txt.Append("Numéro Projet  : ");
                        txt.Append(proj.num_projet);
                        txt.AppendLine();
                        txt.Append("Type de projet  : ");
                        txt.Append(proj.type_projet);
                        txt.AppendLine();
                        txt.Append("Financement : ");
                        txt.Append(proj.financement);
                        txt.AppendLine();
                        txt.Append("Organisme : ");
                        txt.Append(orgProj.nom_organisme);
                        txt.AppendLine();
                        txt.Append("Mail responsable projet  : ");
                        txt.Append(proj.mailRespProjet);
                        txt.AppendLine();
                        txt.Append("Provenance Projet  : ");
                        txt.Append(proj.provenance);
                        txt.AppendLine();
                        txt.Append("Description Projet  : ");
                        txt.Append(proj.description_projet);
                        txt.AppendLine();
                        txt.Append("Date création projet  : ");
                        txt.Append(proj.date_creation);
                        txt.AppendLine();
                        txt.Append('\t');
                        txt.AppendLine();

                        txt.Append("-------INFOS ESSAI-------");
                        txt.AppendLine();
                        txt.Append("Titre essai  : ");
                        txt.Append(essai.titreEssai);
                        txt.AppendLine();
                        txt.Append("ID essai  : ");
                        txt.Append(essai.id);
                        txt.AppendLine();
                        txt.Append("Date Création  : ");
                        txt.Append(essai.date_creation);
                        txt.AppendLine();
                        txt.Append("Type de produit entrant  : ");
                        txt.Append(essai.type_produit_entrant);
                        txt.AppendLine();
                        txt.Append("Précision produit : ");
                        txt.Append(essai.precision_produit);
                        txt.AppendLine();
                        txt.Append("Quantité produit  : ");
                        txt.Append(essai.quantite_produit);
                        txt.AppendLine();
                        txt.Append("Provenance produit  : ");
                        txt.Append(essai.provenance_produit);
                        txt.AppendLine();
                        txt.Append("Destination produit  : ");
                        txt.Append(essai.destination_produit);
                        txt.AppendLine();
                        txt.Append("Date validation essai : ");
                        txt.Append(essai.date_validation);
                        txt.AppendLine();
                        txt.Append("Confidentialité : ");
                        txt.Append(essai.confidentialite);
                        txt.AppendLine();
                        #endregion

                        var txtEssai = File(new System.Text.UTF8Encoding().GetBytes(txt.ToString()), "text/plain", titreTxt);
                        // Sauvegarde dans la base des données
                        doc_essai_pgd docTXT = new doc_essai_pgd
                        {
                            contenu_document = txtEssai.FileContents,
                            nom_document = txtEssai.FileDownloadName,
                            date_creation = DateTime.Now,
                            essaiID = model.idEssai,
                            type_activiteID = 23, // Autres
                            type_documentID = 6 // Autre format
                        };

                        IsDocStock = entrepotDB.SaveDocEssaiPgd(docTXT, ".txt infos essai");
                        if (!IsDocStock)
                        {
                            ViewBag.AfficherMessage = true;
                            ViewBag.Message = "Problème d'écriture dans la base de données du fichier .txt infos essai";
                            return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                        }
                        #endregion

                        // déclarer que l'essai a un entrepot des documents 
                        entrepotDB.UpdateEssaiXEntrepot(model.idEssai);
                        if (proj.date_creation_entrepot == null) // si entrepôt jamais crée alors on rajoute la date de création
                        {
                            // Rajouter la date de création de l'entrepot pour le projet
                            bool isOk = entrepotDB.SaveDateCreationEntrepot(proj.id);
                            if (!isOk)
                            {
                                ViewBag.AfficherMessage = true;
                                ViewBag.Message = "Problème d'écriture de la date création entrepot pour ce projet";
                                return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                            }
                        }
                        return View("ConfirmationEntrepot");
                       
                    }
                    else
                    {
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Problème d'écriture des documents ajoutés pour sauvegarde dans la base de données";
                        return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                    }
                }
                else
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème d'écriture des documents ajoutés pour sauvegarde dans la base de données";
                    return View("CreationEntrepotXEssai", model); // Si error alors on recharge la page pour montrer les messages
                }
            }            
        }

        /// <summary>
        /// Action pour ajout des documents dans un entrepot déjà crée (modification)
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult AjoutDocsDansEntrepot(int? id)
        {
            bool IsAddDoc = false;
            var essai = entrepotDB.ObtenirEssai(id.Value);
            if (essai.entrepot_cree == true)
                IsAddDoc = true;
            else
                IsAddDoc = false;

            // Initialisation de session pour l'essai X
            CreationEntrepotVM vm = new CreationEntrepotVM()
            {
                ListReservationsXEssai = entrepotDB.ListeReservationsXEssai(id.Value),
                idEssai = id.Value,
                TitreEssai = entrepotDB.ObtenirTitreEssai(id.Value),
                ListeTypeDoc = entrepotDB.ListeTypeDocuments(),
                ListDocsPartieUn = new List<DocAjoutePartieUn>(),
                ListDocsPartieDeux = new List<DocAjoutePartieDeux>(),
                AjoutDocs = IsAddDoc
            };

            this.HttpContext.AddToSession("CreationEntrepotVM", vm);

            return View("CreationEntrepotXEssai", vm);
        }
    }
}
