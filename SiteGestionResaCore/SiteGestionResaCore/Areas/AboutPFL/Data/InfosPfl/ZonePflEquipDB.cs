using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data
{
    public class ZonePflEquipDB : IZonePflEquipDB
    {
        private readonly GestionResaContext context;

        public ZonePflEquipDB(
            GestionResaContext context)
        {
            this.context = context;
        }
        public List<zone> ListeZones()
        {
            return context.zone.ToList();
        }

        /// <summary>
        /// Méthode pour obtenir une liste des équipements pour une zone déterminée
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>Liste des équipements + boolean si équipement réservé ou pas</returns>
        public List<InfosEquipement> ListeEquipementsXZone(int idZone)
        {
            List<InfosEquipement> List = new List<InfosEquipement>();
            //bool cheminFicheMat = false;
            //bool cheminFicheMet = false;
            // Liste des équipements de la zone
            var query = context.equipement.Where(e => e.zoneID == idZone && e.equip_delete != true).ToList();

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

        public string NomZoneXEquipement(int idZone)
        {
            return context.zone.First(z => z.id == idZone).nom_zone;
        }

        public doc_fiche_materiel ObtenirDocMateriel(int idDoc)
        {
            return context.doc_fiche_materiel.First(d => d.id == idDoc);
        }
    }
}

