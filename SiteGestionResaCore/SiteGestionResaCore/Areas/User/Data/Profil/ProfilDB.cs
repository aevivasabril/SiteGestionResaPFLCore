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
                    LienEnquete = "http://147.99.161.143/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + x.IdEssai
                    //LienEnquete = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + x.IdEssai // Lien sur mon ordi (FONCTIONNE!!! :D )
                };
                List.Add(Enquete);
            }
            return List;
        }
    }
}
