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
using SiteReservationGestionPFL.Models;

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
            services.AddIdentity<utilisateur,IdentityRole<int>>();
            services.AddDbContext<GestionResaContext>(opts => opts.UseSqlServer(@"data source=localhost\SQLEXPRESS14;initial catalog=PflStloResaTest;integrated security=True;MultipleActiveResultSets=True"));

            services.AddScoped<IResaDB, ResaDB>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSession();
            services.AddOptions();
            services.Configure<EmailOptions>(Configuration.GetSection("Email"));

            services.AddControllersWithViews();
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "area",
                    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
