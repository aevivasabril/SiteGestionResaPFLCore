/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

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

