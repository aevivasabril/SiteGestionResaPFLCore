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
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.Profil
{
    public class ProfilDB: IProfilDB
    {
        private readonly GestionResaContext context;

        public ProfilDB(
            GestionResaContext context)
        {
            this.context = context;
        }

        public string ObtNomEquipe(int equipeID)
        {
            return context.ld_equipes_stlo.FirstOrDefault(e => e.id == equipeID).nom_equipe;
        }

        public string ObtNomOrganisme(int orgID)
        {
            return context.organisme.FirstOrDefault(o => o.id == orgID).nom_organisme;
        }

        public List<EnquetesNonReponduesXUsr> ObtListEnquetes(utilisateur usr)
        {
            List<EnquetesNonReponduesXUsr> List = new List<EnquetesNonReponduesXUsr>();

            var dateToday = DateTime.Now;
            var enquetes = (from en in context.enquete
                            join es in context.essai on en.essaiId equals es.id into T1
                            from m in T1.DefaultIfEmpty()
                            join us in context.Users on m.compte_userID equals us.Id into T2
                            from s in T2.DefaultIfEmpty()
                            join resa in context.reservation_projet on m.id equals resa.essaiID into T3
                            from r in T3.DefaultIfEmpty()
                            join equip in context.equipement on r.equipementID equals equip.id into T4
                            from e in T4.DefaultIfEmpty()
                            where m.resa_refuse != true && m.resa_supprime != true 
                            && r.date_fin < dateToday && en.reponduEnquete != true && m.resa_supprime != true && m.resa_refuse != true
                            && s.Id == usr.Id// réservations dont la date fin n'est pas supérieure à la date d'aujourd'hui (réservations passées) 
                            select new
                            {
                                IdEnq = en.id,
                                TitreEssai = m.titreEssai,
                                User = s.Email,
                                Auteur = s.Email,
                                IdEssai = m.id,
                                Projet = context.projet.First(p=>p.id == m.projetID)
                            }).Distinct();

            foreach (var x in enquetes)
            {
                EnquetesNonReponduesXUsr Enquete = new EnquetesNonReponduesXUsr()
                {
                    IdEnquete = x.IdEnq,
                    InfoEssai = x.TitreEssai + " (N°" + x.IdEssai + ")",
                    InfoProjet = x.Projet.titre_projet + "(N° " + x.Projet.num_projet + ")",
                    //IdEssai = x.IdEssai,
                    LienEnquete = "http://xxx.xx.xx.xxx/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + x.IdEssai
                    //LienEnquete = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + x.IdEssai 
                };
                List.Add(Enquete);
            }
            return List;
        }
    }
}
