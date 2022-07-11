using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Evenements.Data
{
    public class EvenementDB : IEvenementDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<EvenementDB> logger;

        public EvenementDB(
            GestionResaContext resaDB,
            ILogger<EvenementDB> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        public List<evenement> ListEvenements()
        {
            return resaDB.evenement.Distinct().ToList();
        }

        public bool AjoutEvent(string message)
        {
            try
            {
                evenement eve = new evenement() { message = message, date_creation = DateTime.Now };
                resaDB.evenement.Add(eve);
                resaDB.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                logger.LogError("", "problème lors de l'écriture de l'événement: " +  e.ToString());
                return false;
            }            
        }
        public bool SupprimerEvenement(int idEvent)
        {
            try
            {
                evenement eve = resaDB.evenement.First(e => e.id == idEvent);
                resaDB.evenement.Remove(eve);
                resaDB.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("", "problème lors de la suppression de l'événement: " + e.ToString());
                return false;
            }
        }
    }
}   

