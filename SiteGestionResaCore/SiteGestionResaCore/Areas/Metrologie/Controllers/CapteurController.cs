using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Metrologie.Data.Capteur;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Controllers
{
    [Area("Metrologie")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class CapteurController : Controller
    {
        private readonly ICapteurDB capteurDB;

        public CapteurController(
            ICapteurDB capteurDB)
        {
            this.capteurDB = capteurDB; 
        }

        public IActionResult OperationCapteurs()
        {
            OperationCapteurVM vm = new OperationCapteurVM();
            vm.ListCapteurs = capteurDB.ObtenirListCapteurs();
            this.HttpContext.AddToSession("OperationCapteurVM", vm);

            return View("OperationsCapteur", vm);
        }

        public IActionResult AjouterCapteur()
        {
            AjouterCapteurVM vm = new AjouterCapteurVM();
            IList<equipement> list = capteurDB.ObtListEquipements();
            // Créer la liste des équipements à afficher pour sélection
            var ListEquipItem = list.Select(f => new SelectListItem { Value = f.id.ToString(), Text = "(" + f.numGmao +") " + f.nom});
            var listPeriodicite = new List<SelectListItem>();

            listPeriodicite.Add(new SelectListItem() { Value = "0", Text = "- Sélectionnez la periodicité-" });
            listPeriodicite.Add(new SelectListItem() { Value = "1", Text = "6 mois" });
            listPeriodicite.Add(new SelectListItem() { Value = "2", Text = "1 an" });
            listPeriodicite.Add(new SelectListItem() { Value = "3", Text = "2 ans" });

            vm.PeriodiciteItem = listPeriodicite;
            vm.PiloteItem = ListEquipItem;
            this.HttpContext.AddToSession("AjouterCapteurVM", vm);

            return View("AjouterCapteur", vm);
        }

        public IActionResult ValiderAjoutCapteur(AjouterCapteurVM vm)
        {
            double periodInt = 0.0;
            double periodExt = 0.0;
            AjouterCapteurVM model = HttpContext.GetFromSession<AjouterCapteurVM>("AjouterCapteurVM");
            ModelState.Remove("PiloteItem");
            ModelState.Remove("PeriodiciteItem");

            if (ModelState.IsValid)
            {
                if(vm.DateProchaineVerifInt < DateTime.Now)
                {
                    ModelState.AddModelError("DateProchaineVerifInt", "La date de la prochaine vérification interne ne peut pas être inferieur à la date d'aujourd'hui");
                    goto ERR;
                }
                if (vm.DateProchaineVerifExt < DateTime.Now)
                {
                    ModelState.AddModelError("DateProchaineVerifExt", "La date de la prochaine vérification externe ne peut pas être inferieur à la date d'aujourd'hui");
                    goto ERR;
                }
                if (vm.DateDernierVerifInt > DateTime.Now)
                {
                    ModelState.AddModelError("DateDernierVerifInt", "La date de la dernière vérification métrologie interne ne peut pas être superieur à la date d'aujourd'hui");
                    goto ERR;
                }
                if (vm.DateDernierVerifExt > DateTime.Now)
                {
                    ModelState.AddModelError("DateDernierVerifExt", "La date de la dernière vérification métrologie externe ne peut pas être superieur à la date d'aujourd'hui");
                    goto ERR;
                }
                if (vm.CapteurConforme == false && vm.FacteurCorrectif == null) // demander le renseignement du facteur de correction
                {
                    ModelState.AddModelError("FacteurCorrectif", "Si le capteur est non conforme vous devez indiquer un facteur de correction");
                    goto ERR;
                }
                // Ajouter l'information sur le capteur
                // Calculer la periode métrologie INTERNE
                if (vm.SelectPeriodIDint == 1)
                    periodInt = 0.5;
                else if (vm.SelectPeriodIDint == 2)
                    periodInt = 1;
                else
                    periodInt = 2;

                // Ajouter l'information sur le capteur
                // Calculer la periode métrologie Externe
                if (vm.SelectPeriodIDExt == 1)
                    periodExt = 0.5;
                else if (vm.SelectPeriodIDExt == 2)
                    periodExt = 1;
                else
                    periodExt = 2;

                bool IsOk = capteurDB.AjouterCapteur(vm.NomCapteur, vm.CodeCapteur, vm.SelectedPiloteID, vm.DateProchaineVerifInt.Value, vm.DateProchaineVerifExt.Value,
                    vm.DateDernierVerifInt.GetValueOrDefault(), vm.DateDernierVerifExt.GetValueOrDefault(), periodInt, periodExt, vm.CapteurConforme.GetValueOrDefault(),
                    vm.EmtCapteur.GetValueOrDefault(), vm.FacteurCorrectif.GetValueOrDefault());

                if(IsOk == false)
                {
                    ModelState.AddModelError("", "Problème pour rajouter le capteur, veuillez reessayer ultérieurement");
                    goto ERR;
                }
                else
                {
                    OperationCapteurVM vmOp = new OperationCapteurVM();
                    vmOp.ListCapteurs = capteurDB.ObtenirListCapteurs();
                    return View("OperationsCapteur", vmOp);
                }
            }
            else
            {
                goto ERR;
            }
           
        ERR: 
            return View("AjouterCapteur", model);                       
        }

        public IActionResult SupprimerCapteur(int? id)
        {
            OperationCapteurVM model = HttpContext.GetFromSession<OperationCapteurVM>("OperationCapteurVM");
            model.ListCapteurs = capteurDB.ObtenirListCapteurs();
            model.IdCapteurXSupp = id.Value;
            capteur capt = capteurDB.ObtenirCapteur(id.Value);
            model.NomCapteurXSupp = capt.nom_capteur;
            ViewBag.modalSupp = "show";
            return View("OperationsCapteur", model);
        }

        public IActionResult ValiderSuppCap(int? id)
        {
            OperationCapteurVM model = HttpContext.GetFromSession<OperationCapteurVM>("OperationCapteurVM");
            // Supprimer capteur
            bool isOk = capteurDB.SupprimerCapteur(id.Value);
            if (isOk == false)
            {
                ModelState.AddModelError("", "Problème pour supprimer le capteur, veuillez reessayer ultérieurement");
                goto ERR;
            }

            model.ListCapteurs = capteurDB.ObtenirListCapteurs();
            this.HttpContext.AddToSession("OperationCapteurVM", model);
        ERR:
            return View("OperationsCapteur", model);
        }

        public IActionResult ModifierCapteur(int? id)
        {
            int itemPeriodInt = 0;
            int itemPeriodExt = 0;
            // recuperer les infos sur le capteur
            capteur capt = capteurDB.ObtenirCapteur(id.Value);
            equipement equip = capteurDB.ObtenirEquipement(capt.equipementID);

            ModifierCapteurVM vm = new ModifierCapteurVM();
            vm.NomEquipement = equip.nom;
            // List periodicité
            var listPeriodicite = new List<SelectListItem>();

            listPeriodicite.Add(new SelectListItem() { Value = "0", Text = "- Sélectionnez la periodicité-" });
            listPeriodicite.Add(new SelectListItem() { Value = "1", Text = "6 mois" });
            listPeriodicite.Add(new SelectListItem() { Value = "2", Text = "1 an" });
            listPeriodicite.Add(new SelectListItem() { Value = "3", Text = "2 ans" });

            // Calculer l'item selon periode de métrologie interne
            if (capt.periodicite_metrologie_int == 0.5)
                itemPeriodInt = 1;
            else if (capt.periodicite_metrologie_int == 1)
                itemPeriodInt = 2;
            else if(capt.periodicite_metrologie_int == 2)
                itemPeriodInt = 3;

            // Calculer l'item selon periode de métrologie externe
            if (capt.periodicite_metrologie_ext == 0.5)
                itemPeriodExt = 1;
            else if (capt.periodicite_metrologie_ext == 1)
                itemPeriodExt = 2;
            else if(capt.periodicite_metrologie_ext == 2)
                itemPeriodExt = 3;

            vm.SelectPeriodIDExt = itemPeriodExt;
            vm.SelectPeriodIDInt = itemPeriodInt;
            vm.idCapteur = id.Value;
            vm.NomCapteur = capt.nom_capteur;
            vm.CodeCapteur = capt.code_capteur;
            vm.DateProchaineVerifInt = capt.date_prochaine_verif_int;
            vm.DateDernierVerifInt = capt.date_derniere_verif_int;
            vm.DateProchaineVerifExt = capt.date_prochaine_verif_ext;
            vm.DateDernierVerifExt = capt.date_derniere_verif_ext;
            vm.CapteurConforme = capt.capteur_conforme;
            vm.EmtCapteur = capt.emt_capteur;
            vm.FacteurCorrectif = capt.facteur_correctif;
            vm.PeriodiciteItem = listPeriodicite;

            this.HttpContext.AddToSession("ModifierCapteurVM", vm);

            return View("ModifierCapteur", vm);
        }

        [HttpPost]
        public IActionResult ModifCapteur(ModifierCapteurVM model, int id)
        {
            bool isOk = false;
            double periodInt = 0.0;
            double periodExt = 0.0;
            //AjouterCapteurVM model = HttpContext.GetFromSession<AjouterCapteurVM>("AjouterCapteurVM");
            ModelState.Remove("PiloteItem");
            ModelState.Remove("PeriodiciteItem");

            // Obtenir capteur
            capteur capt = capteurDB.ObtenirCapteur(id);
            equipement equip = capteurDB.ObtenirEquipement(capt.equipementID);

            model.NomEquipement = equip.nom;

            // List periodicité
            var listPeriodicite = new List<SelectListItem>();

            listPeriodicite.Add(new SelectListItem() { Value = "0", Text = "- Sélectionnez la periodicité-" });
            listPeriodicite.Add(new SelectListItem() { Value = "1", Text = "6 mois" });
            listPeriodicite.Add(new SelectListItem() { Value = "2", Text = "1 an" });
            listPeriodicite.Add(new SelectListItem() { Value = "3", Text = "2 ans" });
            model.PeriodiciteItem = listPeriodicite;

            if (ModelState.IsValid)
            {
                #region Vérification Date prochaine vérif INTERNE!

                if (model.DateProchaineVerifInt > DateTime.Now)
                {
                    // Vérifier si la date a changé
                    if (model.DateProchaineVerifInt != capt.date_prochaine_verif_int)
                    {
                        // Mettre à jour la periodicité
                        isOk = capteurDB.UpdateDateProVerifInt(capt, model.DateProchaineVerifInt.Value);
                        if (!isOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj de la date pour la prochaine vérification interne, reesayez ultérieurement.");
                            goto END;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("DateProchaineVerifInt", "La date de la prochaine vérification interne ne peut pas être inferieur à la date d'aujourd'hui");
                    goto END;
                }

                #endregion

                #region Vérification Date prochaine vérif Externe!

                if (model.DateProchaineVerifExt > DateTime.Now)
                {
                    // Vérifier si la date a changé
                    if (model.DateProchaineVerifExt != capt.date_prochaine_verif_ext)
                    {
                        // Mettre à jour la periodicité
                        isOk = capteurDB.UpdateDateProVerifExt(capt, model.DateProchaineVerifExt.Value);
                        if (!isOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj de la date pour la prochaine vérification Externe, reesayez ultérieurement.");
                            goto END;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("DateProchaineVerifInt", "La date de la prochaine vérification externe ne peut pas être inferieur à la date d'aujourd'hui");
                    goto END;
                }

                #endregion

                #region Vérification date dernière vérification Interne

                if (model.DateDernierVerifInt < DateTime.Now)
                {
                    // Vérifier si la date a changé
                    if (model.DateDernierVerifInt != capt.date_derniere_verif_int)
                    {
                        // Mettre à jour la periodicité
                        isOk = capteurDB.UpdateDateDerniereVerifInt(capt, model.DateDernierVerifInt.Value);
                        if (!isOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj de la date pour la dernière vérification interne, réesayez ultérieurement.");
                            goto END;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("DateDernierVerifInt", "La date de la dernière vérification métrologie interne ne peut pas être superieur à la date d'aujourd'hui");
                    goto END;
                }

                #endregion

                #region Vérification date dernière vérification Externe

                if (model.DateDernierVerifExt < DateTime.Now)
                {
                    // Vérifier si la date a changé
                    if (model.DateDernierVerifExt != capt.date_derniere_verif_ext)
                    {
                        // Mettre à jour la periodicité
                        isOk = capteurDB.UpdateDateDerniereVerifExt(capt, model.DateDernierVerifExt.Value);
                        if (!isOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj de la date pour la dernière vérification externe, réesayez ultérieurement.");
                            goto END;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("DateDernierVerif", "La date de la dernière vérification métrologie externe ne peut pas être superieur à la date d'aujourd'hui");
                    goto END;
                }

                #endregion

                #region Facteur correction si capteur non conforme

                if (model.CapteurConforme == false && (model.FacteurCorrectif == null || model.FacteurCorrectif == 0)) // demander le renseignement du facteur de correction
                {
                    ModelState.AddModelError("FacteurCorrectif", "Si le capteur est non conforme vous devez indiquer un facteur de correction");
                    goto END;
                }
                else if (model.CapteurConforme == false && model.FacteurCorrectif != 0 && (model.FacteurCorrectif != capt.facteur_correctif))
                {
                    // Mettre à jour le facteur de correction
                    isOk = capteurDB.UpdateFacteur(capt, model.FacteurCorrectif.Value);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj du facteur de correction, réesayez ultérieurement.");
                        goto END;
                    }
                }
                else if(model.CapteurConforme == true && capt.facteur_correctif != 0) // Alors supprimer la valeur de facteur de correction
                {
                    // Mettre à jour le facteur de correction
                    isOk = capteurDB.UpdateFacteur(capt, 0);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la mise à zéro du facteur de correction, réesayez ultérieurement.");
                        goto END;
                    }
                    // ligne de code pour recharger la valeur de "facteurCorrectif" à la bonne valeur
                    ModelState.FirstOrDefault(x => x.Key == "FacteurCorrectif").Value.RawValue = 0; 
                }

                #region Conformité du capteur

                if (capt.capteur_conforme != model.CapteurConforme)
                {
                    // Mettre à jour la periodicité
                    isOk = capteurDB.UpdateConformite(capt, model.CapteurConforme.Value);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj de la conformité du capteur, reesayez ultérieurement.");
                        goto END;
                    }
                }
                #endregion

                #endregion

                #region Vérifier si la periode n'a pas changée
                // Ajouter l'information sur le capteur
                // Calculer la periode métrologie interne
                if (model.SelectPeriodIDInt == 1)
                    periodInt = 0.5;
                else if (model.SelectPeriodIDInt == 2)
                    periodInt = 1;
                else
                    periodInt = 2;

                // Calculer la periode métrologie externe
                if (model.SelectPeriodIDExt == 1)
                    periodExt = 0.5;
                else if (model.SelectPeriodIDExt == 2)
                    periodExt = 1;
                else
                    periodExt = 2;


                if(capt.periodicite_metrologie_int != periodInt)
                {
                    // Mettre à jour la periodicité
                    isOk = capteurDB.UpdatePeriodiciteInt(capt, periodInt);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj de la periode de métrologie interne, reesayez ultérieurement.");
                        goto END;
                    }
                }

                if (capt.periodicite_metrologie_ext != periodExt)
                {
                    // Mettre à jour la periodicité
                    isOk = capteurDB.UpdatePeriodiciteExt(capt, periodExt);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj de la periode de métrologie externe, reesayez ultérieurement.");
                        goto END;
                    }
                }
                #endregion

                #region Vérifier si l'EMT capteur a changée

                if (capt.emt_capteur != model.EmtCapteur)
                {
                    // Mettre à jour la periodicité
                    isOk = capteurDB.UpdateEMT(capt, model.EmtCapteur.Value);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj de l'EMT capteur, reesayez ultérieurement.");
                        goto END;
                    }
                }
                #endregion

                #region Vérifier si le code capteur a changé

                if (capt.code_capteur != model.CodeCapteur)
                {
                    // Mettre à jour la periodicité
                    isOk = capteurDB.UpdateCodeCapteur(capt, model.CodeCapteur);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj du code capteur, reesayez ultérieurement.");
                        goto END;
                    }
                }
                #endregion

                #region Vérifier si le nom capteur a changé

                if (capt.nom_capteur != model.NomCapteur)
                {
                    // Mettre à jour la periodicité
                    isOk = capteurDB.UpdateNomCapteur(capt, model.NomCapteur);
                    if (!isOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj du nom capteur, reesayez ultérieurement.");
                        goto END;
                    }
                }
                #endregion
                
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Le capteur " + capt.nom_capteur + " a été mis à jour! ";
            }
            else
            {
                goto END;
            }

        END:
            model.idCapteur = id;
            return View("ModifierCapteur", model);
        }
    }
}
