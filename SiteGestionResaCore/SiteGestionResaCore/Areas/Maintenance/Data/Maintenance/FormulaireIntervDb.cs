using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public class FormulaireIntervDb: IFormulaireIntervDB
    {
        private readonly GestionResaContext context;
        private readonly ILogger<FormulaireIntervDb> logger;
        private readonly UserManager<utilisateur> userManager;

        public FormulaireIntervDb(
            GestionResaContext context,
            ILogger<FormulaireIntervDb> logger,
            UserManager<utilisateur> userManager)
        {
            this.context = context;
            this.logger = logger;
            this.userManager = userManager;
        }
        public List<ld_type_maintenance> List_Type_Maintenances()
        {
            return context.ld_type_maintenance.ToList();
        }
        //TODO: penser à rajouter le groupe "LOGISTADMIN" et à changer la méthode
        public async Task<IList<utilisateur>> List_utilisateurs_logistiqueAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");
        }

        /// <summary>
        /// Retourner un code d'intervention automatiquement à donner à chaque opération
        /// méthode testé! 14/09/2021
        /// </summary>
        /// <returns></returns>
        /// TODO: penser à filtrer par année peut-être car quand on aura plein des maintenances cela peut ralentir le traitement!
        public string CodeOperation()
        {
            string Code = "";
            string ExprRegular = @"[0-9]{5}";
            string ftm = "00000"; // Format pour conversion sur 5 chiffres completant avec 0 les moins significatifs
            // Vérifier s'il existent déjà des opérations maintenance
            // Récupérer la ligne dans la table la plus recente
            var something = context.maintenance.ToList().OrderByDescending(x => x.id).ToList();
            if(something.Count == 0)
            {
                // Première création d'une opération maintenance
                Code = "INTERV00001";
            }
            else
            { // TODO: vérifier que l'integer est bien géneré
                // Il existent déjà des opérations donc il faut recupérer la derniere, filtrer le nom pour extraire le chiffre et incrementer d'un
                Regex Rg = new Regex(ExprRegular);
                Match collect = Rg.Match(something[0].code_operation); // On devrait avoir seulement un match
                // Convertir cette valeur en integer et ajouter un
                var value = Int16.Parse(collect.Value) + 1;
                // Convertir cette chiffre au format 5 chiffres               
                var intCinqChiffres = value.ToString(ftm);
                Code = "INTERV" + intCinqChiffres;               
            }
            return Code;
        }

        public maintenance AjoutMaintenance(MaintenanceViewModel vm)
        {
            var usr = context.Users.First(u => u.Id == vm.SelectedIntervenantID);
            maintenance maintenance = new maintenance
            {
                code_operation = vm.CodeMaintenance,
                type_maintenance = context.ld_type_maintenance.First(m => m.id == vm.SelectedInterventionId).nom_type_maintenance,
                userID = usr.Id,
                intervenant_externe = Convert.ToBoolean(vm.IntervenantExterne),
                nom_intervenant_ext = vm.NomSociete,
                date_saisie = DateTime.Now,
                description_operation = vm.DescriptionInter
            };

            context.maintenance.Add(maintenance);
            context.SaveChanges();

            return maintenance;
        }

        public bool EnregistrerIntervSansZone(EquipementSansZoneVM sansZoneVM, maintenance maint)
        {
            try
            {
                resa_maint_equip_adjacent resaSansZone = new resa_maint_equip_adjacent
                {
                    date_debut = sansZoneVM.DateDebut,
                    date_fin = sansZoneVM.DateFin,
                    maintenanceID = maint.id,
                    nom_equipement = sansZoneVM.DescriptionProbleme,
                    zone_affectee = sansZoneVM.ZoneImpacte
                };

                context.resa_maint_equip_adjacent.Add(resaSansZone);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de l'ajout d'une opération de maintenance");
                return false;
            }

            return true;
        }
        public List<utilisateur> ObtenirListUtilisateursSite()
        {
            return context.Users.Distinct().ToList();
        }

        public void AnnulerEssai(essai ess, string codeMaint)
        {
            var essa = context.essai.First(e => e.id == ess.id);
            essa.resa_refuse = true;
            essa.status_essai = EnumStatusEssai.Refuse.ToString();
            essa.raison_refus = "Essai annulé automatiquement suite à l'intervention N°: " + codeMaint;
            context.SaveChanges();
        }

        public string ObtenirMailUser(int idUser)
        {
            return context.Users.First(i => i.Id == idUser).Email;
        }

        public bool EnregistrerIntervsDansZone(List<EquipementDansZone> EquipDansZone, maintenance maint)
        {
            bool isOk = false;
            // ajouter les interventions dans des zones
            foreach (var i in EquipDansZone)
            {
                try
                {
                    reservation_maintenance resa = new reservation_maintenance
                    {
                        date_debut = i.DateDebutInterv,
                        date_fin = i.DateFinInterv,
                        equipementID = i.IdEquipementXIntervention,
                        maintenanceID = maint.id
                    };
                    context.reservation_maintenance.Add(resa);
                    context.SaveChanges();
                    isOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString(), "Problème lors de l'ajout d'une opération de maintenance");
                    isOk = false;
                }

            }
            return isOk;
        }

        public essai UpdateEssai(int essId)
        {
            return context.essai.First(e => e.id == essId);
        }
    }
}
