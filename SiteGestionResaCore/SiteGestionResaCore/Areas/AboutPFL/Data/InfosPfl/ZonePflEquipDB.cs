﻿using SiteGestionResaCore.Data;
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
            bool cheminFicheMet = false;
            // Liste des équipements de la zone
            var query = context.equipement.Where(e => e.zoneID == idZone).ToList();

            foreach(var equip in query)
            {
                //cheminFicheMat = (equip.cheminFicheMateriel != null);
                //cheminFicheMet = (equip.cheminFicheMetrologie != null);
                List.Add(new InfosEquipement
                {
                    IdEquipement = equip.id,
                    NomEquipement = equip.nom,
                    NumGmaoEquipement = equip.numGmao,
                    //CheminFicheMateriel = cheminFicheMat,
                    //CheminFicheMetrologie = cheminFicheMet
                });
            }
            return List;
        }

        public string NomZoneXEquipement(int idZone)
        {
            return context.zone.First(z => z.id == idZone).nom_zone;
        }

        public string GetCheminFicheMateriel(int idEquipement)
        {
            return context.equipement.First(z => z.id == idEquipement).nom;
        }
        public string GetNomXChemin(string cheminFichier)
        {
            // Appliquer une regex pour extraire uniquement le nom
            string regexPatt = @"([^\\s]+)$";

            Regex Rg = new Regex(regexPatt);
            MatchCollection match = Rg.Matches(cheminFichier);

            return match[0].Groups[1].Value;
        }
    }
}
