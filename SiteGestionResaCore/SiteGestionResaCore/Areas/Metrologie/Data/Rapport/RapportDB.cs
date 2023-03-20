using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public class RapportDB: IRapportDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<IRapportDB> logger;
        private readonly UserManager<utilisateur> userManager;

        public RapportDB(
            GestionResaContext contextDb,
            ILogger<RapportDB> logger,
            UserManager<utilisateur> userManager)
        {
            this.contextDb = contextDb;
            this.logger = logger;
            this.userManager = userManager;
        }

        /// <summary>
        /// Obtenir la liste des équipements qui ont au moins un capteur de déclaré
        /// </summary>
        /// <returns></returns>
        public IList<EquipVsCapteur> GetEquipements()
        {
            List<EquipVsCapteur> List = new List<EquipVsCapteur>();

            var equips = (from equip in contextDb.equipement
                           from capt in contextDb.capteur
                           where (equip.id == capt.equipementID)
                           select equip).Distinct().ToList();
            foreach(var eq in equips)
            {
                var listCapteurs = contextDb.capteur.Where(c => c.equipementID == eq.id).Distinct().ToList();
                foreach(var capt in listCapteurs)
                {
                    EquipVsCapteur eqCapt = new EquipVsCapteur { capteurId = capt.id, nomCapteur = capt.nom_capteur, equipementId = eq.id, nomEquipement = eq.nom, numGmao = eq.numGmao };
                    List.Add(eqCapt);
                }
            }

            return List;
        }

        public capteur ObtenirCapteur(int idCapteur)
        {
            return contextDb.capteur.First(c => c.id == idCapteur);
        }

        public async Task<IList<utilisateur>> ObtenirAdminUsrs()
        {
            return await userManager.GetUsersInRoleAsync("Admin");
        }

        public string NomEquipementXCapteur(int idEquipement)
        {
            return contextDb.equipement.First(e => e.id == idEquipement).nom;
        }

        public bool CreerRapportMetrologie(byte[] data, string nomDoc, int idCapteur, DateTime dateVerif, string TypeRapport, string comment)
        {
            bool IsOk = false;
            try
            {
                // vérifier s'il existe déjà un rapport métrologique du même type, le supprimer avant de rajouter le nouveau
                if (contextDb.rapport_metrologie.Any(r => r.capteurID == idCapteur && r.type_rapport_metrologie == TypeRapport))
                {
                    // supprimer le rapport metrologique
                    rapport_metrologie rap = contextDb.rapport_metrologie.First(r => r.capteurID == idCapteur && r.type_rapport_metrologie == TypeRapport);
                    contextDb.rapport_metrologie.Remove(rap);
                    contextDb.SaveChanges();
                }

                // Une fois supprimé alors rajouter le nouveau
                rapport_metrologie rapport = new rapport_metrologie
                {
                    capteurID = idCapteur,
                    contenu_rapport = data,
                    date_verif_metrologie = dateVerif,
                    type_rapport_metrologie = TypeRapport,
                    nom_document = nomDoc, 
                    commentaire = comment
                };
                contextDb.rapport_metrologie.Add(rapport);
                contextDb.SaveChanges();
                IsOk = true;
            }
            catch (Exception e)
            {
                logger.LogError("Problème lors de la création de la fiche métrologie: " + e.ToString());
                IsOk = false;
            }
           
            return IsOk;
        }

        public bool majCapteurxRapport(bool IsCaptConform, double FacteurCorrectif, DateTime DateVerifMetro, int IdCapteur, string typeMetro)
        {
            bool IsOk = false;
            try
            {
                var capteur = contextDb.capteur.First(c => c.id == IdCapteur);

                if (IsCaptConform == false)
                {
                    capteur.facteur_correctif = FacteurCorrectif;
                    capteur.capteur_conforme = IsCaptConform;
                    contextDb.SaveChanges();
                }
                else
                {
                    capteur.facteur_correctif = 0;
                    capteur.capteur_conforme = IsCaptConform;
                }

                if(typeMetro == "Interne")
                {
                    // Mise à jour date de vérification
                    capteur.date_derniere_verif_int = DateVerifMetro;
                    contextDb.SaveChanges();

                    // Mise à jour date prochaine vérification
                    if (capteur.periodicite_metrologie_int == 0.5) // tous les 6 mois
                    {
                        var ancienneDate = capteur.date_derniere_verif_int;
                        ancienneDate = ancienneDate.Value.AddMonths(6);
                        capteur.date_prochaine_verif_int = ancienneDate;
                        contextDb.SaveChanges();
                    }
                    else if (capteur.periodicite_metrologie_int == 1) // tous les ans
                    {
                        var ancienneDate = capteur.date_derniere_verif_int;
                        ancienneDate = ancienneDate.Value.AddYears(1);
                        capteur.date_prochaine_verif_int = ancienneDate;
                        contextDb.SaveChanges();
                    }
                    else // tous les 2 ans
                    {
                        var ancienneDate = capteur.date_derniere_verif_int;
                        ancienneDate = ancienneDate.Value.AddYears(2);
                        capteur.date_prochaine_verif_int = ancienneDate;
                        contextDb.SaveChanges();
                    }
                    IsOk = true;
                }

                if (typeMetro == "Externe")
                {
                    // Mise à jour date de vérification
                    capteur.date_derniere_verif_ext = DateVerifMetro;
                    contextDb.SaveChanges();

                    // Mise à jour date prochaine vérification
                    if (capteur.periodicite_metrologie_ext == 0.5) // tous les 6 mois
                    {
                        var ancienneDate = capteur.date_derniere_verif_ext;
                        ancienneDate = ancienneDate.Value.AddMonths(6);
                        capteur.date_prochaine_verif_ext = ancienneDate;
                        contextDb.SaveChanges();
                    }
                    else if (capteur.periodicite_metrologie_ext == 1) // tous les ans
                    {
                        var ancienneDate = capteur.date_derniere_verif_ext;
                        ancienneDate = ancienneDate.Value.AddYears(1);
                        capteur.date_prochaine_verif_ext = ancienneDate;
                        contextDb.SaveChanges();
                    }
                    else // tous les 2 ans
                    {
                        var ancienneDate = capteur.date_derniere_verif_ext;
                        ancienneDate = ancienneDate.Value.AddYears(2);
                        capteur.date_prochaine_verif_ext = ancienneDate;
                        contextDb.SaveChanges();
                    }
                    IsOk = true;
                }
            }
            catch(Exception e)
            {
                IsOk = false;
                logger.LogError("Problème lors de la mise à jour du capteur après opération de metrologie " + typeMetro +" : "+ e.ToString());
            }         

            return IsOk;
        }

        public List<CapteurXRapport> ListRapports()
        {
            CapteurXRapport captXrapp = new CapteurXRapport();
            List<CapteurXRapport> list = new List<CapteurXRapport>();

            var rapports = contextDb.rapport_metrologie.Distinct().ToList();
            foreach(var x in rapports)
            {
                var capt = contextDb.capteur.First(c => c.id == x.capteurID);
                var equip = contextDb.equipement.First(e => e.id == capt.equipementID);

                captXrapp = new CapteurXRapport { idCapteur = capt.id, nomCapteur = capt.nom_capteur, nomEquipement = equip.nom, numGmao = equip.numGmao, dateVerif = x.date_verif_metrologie,
                                                  nom_document = x.nom_document, idRapport = x.id, TypeRapport = x.type_rapport_metrologie, Commentaire = x.commentaire, CodeCapteur = capt.code_capteur};
                list.Add(captXrapp);
            }
            return list;
        }

        public rapport_metrologie ObtenirRapport(int idRapport)
        {
            return contextDb.rapport_metrologie.First(r => r.id == idRapport);
        }
    }
}
