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
        //TODO: configurer les méthodes à async!
        public async Task<IList<InfosAffichage>> ObtenirInfosAffichageAsync()
        {
            List<InfosAffichage> list = new List<InfosAffichage>();
            bool conflitExiste = false;
            // Récuperer tous les "essai" en attente de validation
            var essais =  await resaDB.essai.Where(e => e.status_essai == EnumStatusEssai.WaitingValidation.ToString()).Distinct().ToListAsync();

            foreach (var essai in essais)
            {
                // récupérer toutes les réservations du projet 
                var reservations = await resaDB.reservation_projet.Where(r => r.essaiID == essai.id).Distinct().ToListAsync();
                // pour chaque réservation récupérer les infos importantes et les rajouter à la liste
                foreach (var resa in reservations)
                {
                    //id de la zone où se trouve la réservation
                    int zoneIdresa = (from zon in resaDB.zone
                                      from equi in resaDB.equipement
                                      where zon.id == equi.zoneID && equi.id == resa.equipementID
                                      select zon.id).First();

                    // pour chaque essai "RESTREINT" (seule type où il y a la possibilité d'avoir un conflit) determiner s'il y a un conflit
                    var Reg = await (from proj in resaDB.projet
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
                                   (resa.date_debut <= n.date_fin || resa.date_fin <= n.date_fin)) && (z.id == zoneIdresa)
                               select new
                               {
                                   MailResponsablePrj = proj.mailRespProjet,
                                   NumProjet = proj.num_projet,
                                   DateDeb = n.date_debut,
                                   DateFin = n.date_fin,
                                   NomEquipement = e.nom,
                                   ZoneEquipement = z.nom_zone,
                                   idResa = n.id
                               }).AnyAsync();

                    if(Reg==true)
                    {
                        conflitExiste = true;
                        break;
                    }
                    else
                    {
                        conflitExiste = false;
                    }
                }

                InfosAffichage infosEss = new InfosAffichage() { idEssai = essai.id , DateCreation = essai.date_creation , Commentaire = essai.commentaire,                   
                                                        MailUser = resaDB.Users.First(u => u.Id == Convert.ToInt32(essai.compte_userID)).Email, 
                                                        NomProjet = resaDB.projet.First(p => p.id == essai.projetID).titre_projet, 
                                                        NumProjet = resaDB.projet.First(p => p.id == essai.projetID).num_projet, ConflitExist = conflitExiste,
                                                        idProj = essai.projetID};
                list.Add(infosEss);
            }

            return list;
        }

        public InfosEssai ObtenirInfosEssai(int idEssai)
        {            
            var essai = resaDB.essai.First(e=>e.id == idEssai);

            InfosEssai Infos = new InfosEssai
            {
                id = essai.id,
                Commentaire = essai.commentaire,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = resaDB.Users.First(u => u.Id == Convert.ToInt32(essai.compte_userID)).Email,
                PrecisionProd = essai.precision_produit,
                ProveProd = essai.provenance_produit,
                QuantiteProd = essai.quantite_produit,
                TransportStlo  = essai.transport_stlo,
                TypeProduitEntrant = essai.type_produit_entrant
            };
            return Infos;
        }

        public InfosProjet ObtenirInfosProjet(int id)
        {
            var proj = resaDB.projet.First(p => p.id == id);

            InfosProjet infos = new InfosProjet()
            {
                DateCreation = proj.date_creation, Description = proj.description_projet, Financement = proj.financement, MailRespProj = proj.mailRespProjet,
                MailUsrSaisie = resaDB.Users.First(p=>p.Id == Convert.ToInt32(proj.compte_userID)).Email, NumProjet = proj.num_projet, 
                Organisme = resaDB.organisme.First(o=>o.id== proj.organismeID).nom_organisme, Provenance = proj.provenance, TitreProjet = proj.titre_projet,
                TypeProjet = proj.type_projet
            };
            return infos;

        }

        public List<InfosReservation> InfosReservations(int idEssai)
        {
            List<InfosReservation> ListResas = new List<InfosReservation>();
            // récupérer toutes les réservations du projet 
            var reservations = resaDB.reservation_projet.Where(r => r.essaiID == idEssai).Distinct().ToList();

            foreach(var x in reservations)
            {
                InfosReservation resa = new InfosReservation() {DateDebut = x.date_debut, DateFin = x.date_fin,
                    NomEquipement = resaDB.equipement.First(e=>e.id == x.equipementID).nom, 
                    ZoneEquipement =    (from zon in resaDB.zone
                                        from equi in resaDB.equipement
                                        where zon.id == equi.zoneID && equi.id == x.equipementID
                                        select zon.nom_zone).First()
                };
                ListResas.Add(resa);
            }
            return ListResas;
        }

        public List<InfosConflit> InfosConflits(int idEssai)
        {
            List<InfosConflit> ListConflit = new List<InfosConflit>();
            // récupérer toutes les réservations du projet 
            var reservations = resaDB.reservation_projet.Where(r => r.essaiID == idEssai).Distinct().ToList();
            // pour chaque réservation récupérer les infos importantes et les rajouter à la liste
            foreach (var resa in reservations)
            {
                //id de la zone où se trouve la réservation
                int zoneIdresa = (from zon in resaDB.zone
                                  from equi in resaDB.equipement
                                  where zon.id == equi.zoneID && equi.id == resa.equipementID
                                  select zon.id).First();

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
                           where m.confidentialite == EnumConfidentialite.Restreint.ToString() && m.id != idEssai
                           && ((resa.date_debut >= n.date_debut || resa.date_fin >= n.date_debut) &&
                               (resa.date_debut <= n.date_fin || resa.date_fin <= n.date_fin)) && (z.id == zoneIdresa)
                           select new
                           {
                               MailResponsablePrj = proj.mailRespProjet,
                               NumProjet = proj.num_projet,
                               DateDeb = n.date_debut,
                               DateFin = n.date_fin,
                               NomEquipement = e.nom,
                               ZoneEquipement = z.nom_zone,
                               idResa = n.id, 
                               idEss = m.id
                           });

                foreach (var x in Reg)
                {
                    if (!ListConflit.Where(j => j.IdResa == x.idResa).Any()) // vérifier que la réservation n'est pas présente dans la liste
                    {
                        InfosConflit infosConfInterne = new InfosConflit()
                        {
                            IdResa = x.idResa,
                            DateDeb = x.DateDeb,
                            DateFin = x.DateFin,
                            MailResponsablePrj = x.MailResponsablePrj,
                            NomEquipement = x.NomEquipement,
                            NumProjet = x.NumProjet,
                            ZoneEquipement = x.ZoneEquipement, 
                            IdEss = x.idEss
                        };
                        ListConflit.Add(infosConfInterne);
                    }// sinon on rajoute rien à la liste                                                                        
                }
            }          
            return ListConflit;
        }

        public InfosProjet ObtenirInfosProjetFromEssai(int idEssai)
        {
            var proj = resaDB.projet.First(p=>p.id == resaDB.essai.First(e => e.id == idEssai).projetID);

            InfosProjet infos = new InfosProjet()
            {
                DateCreation = proj.date_creation,
                Description = proj.description_projet,
                Financement = proj.financement,
                MailRespProj = proj.mailRespProjet,
                MailUsrSaisie = resaDB.Users.First(p => p.Id == Convert.ToInt32(proj.compte_userID)).Email,
                NumProjet = proj.num_projet,
                Organisme = resaDB.organisme.First(o => o.id == proj.organismeID).nom_organisme,
                Provenance = proj.provenance,
                TitreProjet = proj.titre_projet,
                TypeProjet = proj.type_projet
            };
            return infos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Liste des réservations avec leurs infos pour affichage</returns>
        public List<InfosAffichage> ResasAValider()
        {
            // TODO: Valider la requete en ajoutant une réservation restreint en conflit!
            
            List<InfosConflit> ListConflit = new List<InfosConflit>();
            List<InfosReservation> ListResas = new List<InfosReservation>();
            List<InfosAffichage> list = new List<InfosAffichage>();

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
                /*InfosEssai infosEss = new InfosEssai() { id = essai.id , DateCreation = essai.date_creation , Commentaire = essai.commentaire, Confidentialite = essai.confidentialite,
                                                        MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email, 
                                                        MailUser = resaDB.Users.First(u => u.Id == Convert.ToInt32(essai.compte_userID)).Email,
                                                        TransportStlo = essai.transport_stlo, TypeProduitEntrant = essai.type_produit_entrant, 
                                                        NomProjet = resaDB.projet.First(p => p.id == essai.projetID).titre_projet, 
                                                        NumProjet = resaDB.projet.First(p => p.id == essai.projetID).num_projet,
                                                        InfosConflits = ListConflit, Reservations = ListResas };*/
                //list.Add(infosEss);
            }

            return list;
        }
    }
}
