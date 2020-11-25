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
        public List<InfosEssai> ResasAValider()
        {
            // TODO: Valider la requete en ajoutant une réservation restreint en conflit!
            
            List<InfosConflit> ListConflit = new List<InfosConflit>();
            List<InfosReservation> ListResas = new List<InfosReservation>();
            List<InfosEssai> list = new List<InfosEssai>();

            // Récuperer tous les "essai" en attente de validation
            var essais = resaDB.essai.Where(e => e.status_essai == EnumStatusEssai.WaitingValidation.ToString()).Distinct().ToList();
            
            foreach(var essai in essais)
            {
                
                // récupérer toutes les réservations du projet 
                var reservations = resaDB.reservation_projet.Where(r => r.essaiID == essai.id).Distinct().ToList();
                // pour chaque réservation récupérer les infos importantes et les rajouter à la liste
                foreach(var resa in reservations)
                {
                    // Recupere les données réservation à afficher
                    InfosReservation infosResas = new InfosReservation() { DateDebut = resa.date_debut, DateFin = resa.date_fin, 
                                                            NomEquipement = resaDB.equipement.First(e => e.id == resa.equipementID).nom,
                                                            ZoneEquipement = (from zon in resaDB.zone
                                                                              from equi in resaDB.equipement
                                                                              where zon.id == equi.zoneID && equi.id == resa.equipementID
                                                                              select zon.nom_zone).First()
                                                            };
                    //id de la zone où se trouve la réservation
                    int zoneIdresa = (from zon in resaDB.zone
                                  from equi in resaDB.equipement
                                  where zon.id == equi.zoneID && equi.id == resa.equipementID
                                  select zon.id).First();
                    // ajouter les infons réservation à la liste
                    ListResas.Add(infosResas);

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
                                   ZoneEquipement = z.nom_zone,
                                   idResa = n.id
                               });

                    foreach(var x in Reg)
                    {
                        if(!ListConflit.Where(j => j.IdResa == x.idResa).Any()) // vérifier que la réservation n'est pas présente dans la liste
                        {
                            InfosConflit infosConfInterne = new InfosConflit() { IdResa = x.idResa, DateDeb = x.DateDeb, DateFin = x.DateFin, 
                                                                                MailResponsablePrj = x.MailResponsablePrj, NomEquipement = x.NomEquipement,
                                                                                NumProjet = x.NumProjet, ZoneEquipement = x.ZoneEquipement};
                            ListConflit.Add(infosConfInterne);
                        }// sinon on rajoute rien à la liste                                                                        
                    }
                }
                // Initializer l'objet contenant toutes les infos des essais
                InfosEssai infosEss = new InfosEssai() { id = essai.id , DateCreation = essai.date_creation , Commentaire = essai.commentaire, Confidentialite = essai.confidentialite,
                                                        MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email, 
                                                        MailUser = resaDB.Users.First(u => u.Id == Convert.ToInt32(essai.compte_userID)).Email,
                                                        TransportStlo = essai.transport_stlo, TypeProduitEntrant = essai.type_produit_entrant, 
                                                        NomProjet = resaDB.projet.First(p => p.id == essai.projetID).titre_projet, 
                                                        NumProjet = resaDB.projet.First(p => p.id == essai.projetID).num_projet,
                                                        InfosConflits = ListConflit, Reservations = ListResas };
                list.Add(infosEss);
            }

            return list;
        }
    }
}
