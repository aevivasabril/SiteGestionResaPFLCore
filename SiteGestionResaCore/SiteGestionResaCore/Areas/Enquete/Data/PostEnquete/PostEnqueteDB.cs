using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data.PostEnquete
{
    public class PostEnqueteDB: IPostEnqueteDB
    {
        private readonly GestionResaContext resaDb;

        public PostEnqueteDB(
            GestionResaContext resaDb)
        {
            this.resaDb = resaDb;
        }

        /// <summary>
        /// Si l'enquete appartient à un essai finalisé entre ces 2 dates, et que l'enquete a été répondue alors on récupére la réponse
        /// </summary>
        /// <param name="datedu"></param>
        /// <param name="dateau"></param>
        /// <returns> Liste des enquetes répondues</returns>
        public List<enquete> GetReponsesEnquetes(DateTime datedu, DateTime dateau)
        {
            // TODO: vérifier
            return resaDb.enquete.Where(e => e.date_premier_envoi.Value.Date >= datedu.Date && e.date_premier_envoi.Value.Date <= dateau.Date && e.reponduEnquete == true).ToList();
        }

        public essai GetEssai(int IdEssai)
        {
            return resaDb.essai.First(e => e.id == IdEssai);
        }

        public projet GetProjet(int IdProjet)
        {
            return resaDb.projet.First(p => p.id == IdProjet);
        }

        public List<EnquetesSansReponse> ObtEnquetesSansRp()
        {
            List<EnquetesSansReponse> List = new List<EnquetesSansReponse>();

            var dateToday = DateTime.Now;
            var enquetes = (from en in resaDb.enquete
                            join es in resaDb.essai on en.essaiId equals es.id into T1
                            from m in T1.DefaultIfEmpty()
                            join us in resaDb.Users on m.compte_userID equals us.Id into T2
                            from s in T2.DefaultIfEmpty()
                            join resa in resaDb.reservation_projet on m.id equals resa.essaiID into T3
                            from r in T3.DefaultIfEmpty()
                            join equip in resaDb.equipement on r.equipementID equals equip.id into T4
                            from e in T4.DefaultIfEmpty()
                            where m.resa_refuse != true && m.resa_supprime != true && r.date_debut.Year == dateToday.Year && r.date_fin.Year == dateToday.Year // reservations de l'année
                            && r.date_fin < dateToday && en.reponduEnquete != true && m.resa_supprime != true && m.resa_refuse != true // réservations dont la date fin n'est pas supérieure à la date d'aujourd'hui (réservations passées) 
                            select new
                            {
                                IdEnq = en.id,
                                TitreEssai = m.titreEssai,
                                User = s.Email,
                                Auteur = s.Email,
                                IdEssai = m.id
                            }).Distinct();
            
            foreach (var x in enquetes)
            {
                EnquetesSansReponse Enquete = new EnquetesSansReponse()
                {
                    IdEnquete = x.IdEnq,
                    TitreEssai = x.TitreEssai,
                    AuteurEssai = x.Auteur,
                    IdEssai = x.IdEssai
                };
                List.Add(Enquete);                                                                     
            }
            return List;
        }

        public enquete GetEnqueteXEssai(int IdEssai)
        {
            return resaDb.enquete.First(e => e.essaiId == IdEssai);
        }
    }
}
