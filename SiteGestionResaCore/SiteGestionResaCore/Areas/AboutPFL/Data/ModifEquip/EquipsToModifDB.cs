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

        public EquipsToModifDB(
            GestionResaContext context)
        {
            this.context = context;
        }
        public List<InfosEquipement> ListeEquipementsXZone(int idZone)
        {
            List<InfosEquipement> List = new List<InfosEquipement>();
            bool cheminFicheMat = false;
            //bool cheminFicheMet = false;
            // Liste des équipements de la zone
            var query = context.equipement.Where(e => e.zoneID == idZone).ToList();

            foreach (var equip in query)
            {
                // Vérifier s'il existe une fiche materiel pour cet équipement
                var fiche = context.doc_fiche_materiel.FirstOrDefault(d => d.equipementID == equip.id);
                
                if (fiche == null)
                    cheminFicheMat = false;
                else
                    cheminFicheMat = true;
                //cheminFicheMat = (equip.cheminFicheMateriel != null);
                //cheminFicheMet = (equip.cheminFicheMetrologie != null);
                List.Add(new InfosEquipement
                {
                    IdEquipement = equip.id,
                    NomEquipement = equip.nom,
                    NumGmaoEquipement = equip.numGmao,
                    FicheMaterielDispo = cheminFicheMat,
                    //CheminFicheMetrologie = cheminFicheMet
                });
            }
            return List;
        }
    }
}
