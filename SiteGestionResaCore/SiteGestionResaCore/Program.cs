using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data.Data;

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
