using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Opts;

namespace SiteGestionResaCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostReturn = CreateHostBuilder(args).Build(); // appel à configure services 
            using(var scope = hostReturn.Services.CreateScope())
            {
                using (var db = scope.ServiceProvider.GetRequiredService<GestionResaContext>())
                {
                    await db.Database.MigrateAsync();// Vérifie les changements sur la BDD prod et dev et le mettre à jour 
                
                    IOptions<AdminOptions> options = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>();
                    var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<utilisateur>>();
                    if(await UserManager.FindByEmailAsync(options.Value.email) == null)
                    {
                        var usr = new utilisateur { Email = options.Value.email, UserName = options.Value.email, nom = options.Value.nom, prenom = options.Value.prenom, EmailConfirmed = true };
                        await UserManager.CreateAsync(usr, options.Value.mdp);
                        await UserManager.AddToRoleAsync(usr, "MainAdmin");
                        await UserManager.AddToRoleAsync(usr, "Admin");
                    }
                }
            }
            hostReturn.Run(); // appel à configure

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
