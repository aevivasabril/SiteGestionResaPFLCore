using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<utilisateur> userManager;

        public FormulaireIntervDb(
            GestionResaContext context,
            UserManager<utilisateur> userManager)
        {
            this.context = context;
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

        // TODO: vérifier cette méthode!! 
        /// <summary>
        /// Retourner un code d'intervention automatiquement à donner à chaque opération
        /// </summary>
        /// <returns></returns>
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
    }
}
