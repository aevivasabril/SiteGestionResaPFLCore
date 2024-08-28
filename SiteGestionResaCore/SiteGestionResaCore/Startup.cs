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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Opts;
using SiteGestionResaCore.Services;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Areas.Reservation.Data.Consultation;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Areas.Equipe.Data;
using SiteGestionResaCore.Areas.Calendrier.Data;
using SiteGestionResaCore.Data.PcVue;
using SiteGestionResaCore.Areas.User.Data.DonneesUser;
using SiteGestionResaCore.Areas.Equipe.Data.RecupData;
using SiteGestionResaCore.Areas.Enquete.Data;
using SiteGestionResaCore.Services.ScheduleTask;
using SiteGestionResaCore.Areas.Enquete.Data.PostEnquete;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Areas.Maintenance.Data.Consultation;
using SiteGestionResaCore.Areas.Maintenance.Data.Modification;
using SiteGestionResaCore.Areas.AboutPFL.Data;
using SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite;
using SiteGestionResaCore.Areas.DonneesPGD.Data;
using SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot;
using SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq;
using SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip;
using SiteGestionResaCore.Areas.StatistiquePFL.Data;
using SiteGestionResaCore.Areas.Evenements.Data;
using SiteGestionResaCore.Areas.Metrologie.Data;
using SiteGestionResaCore.Areas.Metrologie.Data.Capteur;
using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
using SiteGestionResaCore.Areas.User.Data.Profil;

namespace SiteGestionResaCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<utilisateur,IdentityRole<int>>().AddEntityFrameworkStores<GestionResaContext>().AddUserManager<UserManager<utilisateur>>().AddRoleManager<RoleManager<IdentityRole<int>>>().AddDefaultTokenProviders();
            services.AddDbContext<GestionResaContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            services.AddDbContext<PcVueContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("PcVue")));


            //services.AddScoped<IResaDB, ResaDB>();
            services.AddScoped<IAccountResaDB, AccountResaDB>();
            services.AddScoped<IEquipeResaDb, EquipeResaDb>();
            services.AddScoped<IFormulaireResaDb, FormulaireResaDb>();
            services.AddScoped<IProjetEssaiResaDb, ProjetEssaiResaDb>();
            services.AddScoped<IZoneEquipDb, ZoneEquipDb>();
            services.AddScoped<IReservationDb, ReservationDb>();
            services.AddScoped<IResaAValiderDb, ResaAValiderDb>();
            services.AddScoped<IConsultResasDB, ConsultResasDB>();
            services.AddScoped<IResasUserDB, ResasUserDB>();
            services.AddScoped<ICalendResaDb, CalendResaDb>();
            services.AddScoped<IDonneesUsrDB, DonneesUsrDB>();
            services.AddScoped<IDataAdminDB, DataAdminDB>();
            services.AddScoped<IEnqueteDb, EnqueteDb>();
            services.AddScoped<IEnqueteTaskDB, EnqueteTaskDB>();
            services.AddScoped<IPostEnqueteDB, PostEnqueteDB>();
            services.AddScoped<IFormulaireIntervDB, FormulaireIntervDb>();
            services.AddScoped<IConsultMaintDb, ConsultMaintDb>();
            services.AddScoped<IModifMaintenanceDB, ModifMaintenanceDB>();
            services.AddScoped<IZonePflEquipDB, ZonePflEquipDB>();
            services.AddScoped<IDocsQualiDB, DocsQualiDB >();
            services.AddScoped<IEssaisXEntrepotDB, EssaisXEntrepotDB>();
            services.AddScoped<IModifAqDB, ModifAqDB>();
            services.AddScoped<IAccesEntrepotDB, AccesEntrepotDB>();
            services.AddScoped<IEquipsToModifDB, EquipsToModifDB>();
            services.AddScoped<IEntrepotTaskDB, EntrepotTaskDB>();
            services.AddScoped<IStatistiquesDB, StatistiquesDB>();
            services.AddScoped<IEvenementDB, EvenementDB>();
            services.AddScoped<IDocMetroDB, DocMetroDB>();
            services.AddScoped<ICapteurDB, CapteurDB>();
            services.AddScoped<IRapportDB, RapportDB>();
            services.AddScoped<IRapportMetroDB, RapportMetroDB>();
            services.AddScoped<IProfilDB, ProfilDB>();

            services.AddSingleton<IEmailSender, EmailSender>();
            //services.AddScoped<IReportGenerator, ReportGenerator>();
            services.AddSingleton<IHostedService, EnqueteTask>();
            services.AddSingleton<IHostedService, EntrepotTask>();
            services.AddSingleton<IHostedService, RapportMetroTask>();


            services.AddSession();
            services.AddOptions();
            services.Configure<EmailOptions>(Configuration.GetSection("Email"));
            services.Configure<AdminOptions>(Configuration.GetSection("MainAdmin"));

            services.AddControllersWithViews()
            #if DEBUG
                .AddRazorRuntimeCompilation() // Si on est en debug et que on fait des changements sur une vue en cliquant f5 on peut recharger la page!
            #endif
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Views/Shared/Error");
            }

            //app.UseStatusCodePages();
            // Rediger les personnes non autorisés vers la page d'accueil
            app.UseStatusCodePagesWithRedirects("~/Home/Index");
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "area",
                    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
