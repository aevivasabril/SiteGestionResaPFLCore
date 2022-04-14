using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip
{
    public class EquipsToModifDB: IEquipsToModifDB
    {
        private readonly GestionResaContext context;
        private readonly ILogger<EquipsToModifDB> logger;

        public EquipsToModifDB(
            GestionResaContext context,
            ILogger<EquipsToModifDB> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public List<InfosEquipement> ListeEquipementsXZone(int idZone)
        {
            List<InfosEquipement> List = new List<InfosEquipement>();

            // Liste des équipements de la zone
            var query = context.equipement.Where(e => e.zoneID == idZone).ToList();

            foreach (var equip in query)
            {
                // Vérifier s'il existe une fiche materiel pour cet équipement
                var fiche = context.doc_fiche_materiel.FirstOrDefault(d => d.equipementID == equip.id);
                
                if (fiche == null)
                {
                    List.Add(new InfosEquipement
                    {
                        IdEquipement = equip.id,
                        NomEquipement = equip.nom,
                        NumGmaoEquipement = equip.numGmao,
                        FicheMaterielDispo = false
                    });
                }
                else
                {
                    List.Add(new InfosEquipement
                    {
                        IdEquipement = equip.id,
                        NomEquipement = equip.nom,
                        NumGmaoEquipement = equip.numGmao,
                        FicheMaterielDispo = true,
                        NomFicheMat = fiche.nom_document,
                        DateModif = fiche.date_modification,
                        IdFicheMat = fiche.id
                    });
                }              
            }
            return List;
        }
        public bool AjouterFicheXEquipement(int idEquipement, byte[] content, string nom)
        {
            bool IsOk = false;

            try
            {
                doc_fiche_materiel doc = new doc_fiche_materiel
                {
                    contenu_fiche = content,
                    date_modification = DateTime.Now,
                    nom_document = nom,
                    equipementID = idEquipement
                };
                context.doc_fiche_materiel.Add(doc);

                context.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                logger.LogError("Problème lors de l'écriture de la fiche materiel: " + nom + "dans la BDD: " + e.ToString());
                IsOk = false;
            }
            return IsOk;
        }
    }
}
