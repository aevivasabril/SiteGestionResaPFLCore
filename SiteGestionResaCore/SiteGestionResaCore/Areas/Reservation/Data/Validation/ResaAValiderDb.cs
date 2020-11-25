using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    /// <summary>
    /// Classe d'accès à la base de données pour la partie validation des réservations
    /// </summary>
    public class ResaAValiderDb : IResaAValiderDb
    {
        private readonly GestionResaContext resaDB;

        public ResaAValiderDb(
            GestionResaContext resaDB)
        {
            this.resaDB = resaDB;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Liste des réservations avec leurs infos pour affichage</returns>
        public List<AffichageResasAValider> ResasAValider()
        {
            // TODO: Valider la requete en ajoutant une réservation restreint en conflit!
            AffichageResasAValider Essai = new AffichageResasAValider();
            InfosReservation infosResas = new InfosReservation();
            InfosEssai infosEss = new InfosEssai();
            InfosConflit infosConfInterne = new InfosConflit();
            List<AffichageResasAValider> list = new List<AffichageResasAValider>();

            // Récuperer tous les "essai" en attente de validation
            var essais = resaDB.essai.Where(e => e.status_essai == EnumStatusEssai.WaitingValidation.ToString()).Distinct().ToList();
            
            foreach(var essai in essais)
            {
                // récupérer les infos nécessaires sur chaque essai
                infosEss.Commentaire = essai.commentaire;
                infosEss.Confidentialite = essai.confidentialite;
                infosEss.DateCreation = essai.date_creation;
                infosEss.id = essai.id;
                infosEss.MailManipulateur = resaDB.Users.First( u => u.Id == essai.manipulateurID).Email;
                infosEss.MailUser = resaDB.Users.First(u=>u.Id == Convert.ToInt32(essai.compte_userID)).Email;
                infosEss.TransportStlo = essai.transport_stlo;
                infosEss.TypeProduitEntrant = essai.type_produit_entrant;
                // récupérr les infos sur le projet associé
                infosEss.NomProjet = resaDB.projet.First(p=> p.id == essai.projetID).titre_projet;
                infosEss.NumProjet = resaDB.projet.First(p => p.id == essai.projetID).num_projet;
                // récupérer toutes les réservations du projet 
                var reservations = resaDB.reservation_projet.Where(r => r.essaiID == essai.id).Distinct().ToList();
                // pour chaque réservation récupérer les infos importantes et les rajouter à la liste
                foreach(var resa in reservations)
                {
                    infosResas.DateDebut = resa.date_debut;
                    infosResas.DateFin = resa.date_fin;
                    infosResas.NomEquipement = resaDB.equipement.First(e=> e.id == resa.equipementID).nom;
                    infosResas.ZoneEquipement = (from zon in resaDB.zone
                                                from equi in resaDB.equipement
                                                where zon.id == equi.zoneID && equi.id == resa.equipementID
                                                select zon.nom_zone).First();
                    //id de la zone où se trouve la réservation
                    int zoneIdresa = (from zon in resaDB.zone
                                  from equi in resaDB.equipement
                                  where zon.id == equi.zoneID && equi.id == resa.equipementID
                                  select zon.id).First();
                    infosEss.Reservations.Add(infosResas);

                    // pour chaque essai "RESTREINT" (seule type où il y a la possibilité d'avoir un conflit) alors trouver les infos de chaque réservation ayant un conflit
                    // requete jointure ne fonctionne pas sur entity framework
                    var Reg = (from proj in resaDB.projet
                               join ess in resaDB.essai on proj.id equals ess.projetID into t1
                               from m in t1.DefaultIfEmpty()
                               join res in resaDB.reservation_projet on m.id equals res.essaiID into t2
                               from n in t2.DefaultIfEmpty()
                               join equi in resaDB.equipement on n.equipementID equals equi.id into t3
                               from e in t3.DefaultIfEmpty()
                               join zo in resaDB.zone on e.zoneID equals zo.id into t4
                               from z in t4.DefaultIfEmpty()
                               where m.confidentialite == EnumConfidentialite.Restreint.ToString() && m.id != essai.id
                               && ((resa.date_debut >= n.date_debut || resa.date_fin >= n.date_debut) &&
                                   (resa.date_debut <= n.date_fin || resa.date_fin <= n.date_fin) ) && (z.id == zoneIdresa)
                               select new 
                               {
                                   MailResponsablePrj = proj.mailRespProjet,
                                   NumProjet = proj.num_projet,
                                   DateDeb = n.date_debut,
                                   DateFin = n.date_fin,
                                   NomEquipement = e.nom,
                                   ZoneEquipement = z.nom_zone
                               });

                    foreach(var x in Reg)
                    {
                        
                        infosConfInterne.MailResponsablePrj = x.MailResponsablePrj;
                        infosConfInterne.NomEquipement = x.NomEquipement;
                        infosConfInterne.NumProjet = x.NumProjet;
                        infosConfInterne.ZoneEquipement = x.ZoneEquipement;
                        infosConfInterne.DateDeb = x.DateDeb;
                        infosConfInterne.DateFin = x.DateFin;

                        infosEss.InfosConflits.Add(infosConfInterne);
                                               
                    }
                }
                Essai.InfosEssai = infosEss;
                list.Add(Essai);
            }

            return list;
        }
    }
}
