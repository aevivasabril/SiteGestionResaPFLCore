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

            services.AddSingleton<IEmailSender, EmailSender>();

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
                app.UseExceptionHandler("/Home/Error");
            }
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
