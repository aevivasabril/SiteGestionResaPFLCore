using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    /// <summary>
    /// Classe d'accès à la base de données pour la partie validation des réservations
    /// </summary>
    public class ResaAValiderDb : IResaAValiderDb
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<ResaAValiderDb> logger;

        public ResaAValiderDb(
            GestionResaContext resaDB, 
            ILogger<ResaAValiderDb> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        //TODO: configurer les méthodes à async!
        /// <summary>
        /// Methode permettant d'obtenir diverses infos sur chaque essai pour 
        /// affichage sur la page principal de réservations à valider
        /// </summary>
        /// <returns>Liste des infos pour affichage</returns>
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
                               where m.confidentialite == EnumConfidentialite.Restreint.ToString() && m.id != essai.id &&
                               (m.status_essai != EnumStatusEssai.Refuse.ToString()) && (m.status_essai != EnumStatusEssai.Canceled.ToString())
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

                InfosAffichage infosEss = new InfosAffichage() { idEssai = essai.id , DateCreation = essai.date_creation , TitreEssai = essai.titreEssai,                   
                                                        MailUser = resaDB.Users.First(u => u.Id == essai.compte_userID).Email, 
                                                        NomProjet = resaDB.projet.First(p => p.id == essai.projetID).titre_projet, 
                                                        NumProjet = resaDB.projet.First(p => p.id == essai.projetID).num_projet, ConflitExist = conflitExiste,
                                                        idProj = essai.projetID};
                list.Add(infosEss);
            }

            return list.OrderByDescending(x=>x.DateCreation).ToList();
        }

        /// <summary>
        /// Obtenir les infos affichage essai à partir d'un id essai
        /// </summary>
        /// <param name="idEssai"></param>
        /// <returns></returns>
        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai)
        {            
            var essai = resaDB.essai.First(e=>e.id == idEssai);

            ConsultInfosEssaiChildVM Infos = new ConsultInfosEssaiChildVM
            {
                id = essai.id,
                TitreEssai = essai.titreEssai,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = resaDB.Users.First(u => u.Id == essai.compte_userID).Email,
                PrecisionProd = essai.precision_produit,
                ProveProd = essai.provenance_produit,
                QuantiteProd = essai.quantite_produit,
                TransportStlo  = essai.transport_stlo,
                TypeProduitEntrant = essai.type_produit_entrant
            };
            return Infos;
        }

        /// <summary>
        /// Obtenir des infos sur un projet à partir son Id
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public InfosProjet ObtenirInfosProjet(int id)
        {
            var proj = resaDB.projet.First(p => p.id == id);

            InfosProjet infos = new InfosProjet()
            {
                DateCreation = proj.date_creation, Description = proj.description_projet, Financement = proj.financement, MailRespProj = proj.mailRespProjet,
                MailUsrSaisie = resaDB.Users.First(p=>p.Id == proj.compte_userID).Email, NumProjet = proj.num_projet, 
                Organisme = resaDB.organisme.First(o=>o.id== proj.organismeID).nom_organisme, Provenance = proj.provenance, TitreProjet = proj.titre_projet,
                TypeProjet = proj.type_projet
            };
            return infos;

        }

        /// <summary>
        /// Obtenir toutes les réservations pour un essai
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns>liste des réservations</returns>
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

        /// <summary>
        /// Méthode pour calculer les possibles conflits pour un essai
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns>Liste des infos sur les réservations en conflit</returns>
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
                // eviter de vérifier les conflits sur des autres essais refusés ou annulés
                var Reg = (from proj in resaDB.projet
                           join ess in resaDB.essai on proj.id equals ess.projetID into t1
                           from m in t1.DefaultIfEmpty()
                           join res in resaDB.reservation_projet on m.id equals res.essaiID into t2
                           from n in t2.DefaultIfEmpty()
                           join equi in resaDB.equipement on n.equipementID equals equi.id into t3
                           from e in t3.DefaultIfEmpty()
                           join zo in resaDB.zone on e.zoneID equals zo.id into t4
                           from z in t4.DefaultIfEmpty()
                           where m.confidentialite == EnumConfidentialite.Restreint.ToString() && m.id != idEssai && 
                           ( m.status_essai != EnumStatusEssai.Refuse.ToString() ) && ( m.status_essai != EnumStatusEssai.Canceled.ToString() )
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
                               idEss = m.id,
                               TitreEss = m.titreEssai
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
                            IdEss = x.idEss,
                            TitreEss = x.TitreEss
                        };
                        ListConflit.Add(infosConfInterne);
                    }// sinon on rajoute rien à la liste                                                                        
                }
            }          
            return ListConflit;
        }

        /// <summary>
        /// Obtenir infos sur un projet à partir d'un essai id
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns>InfosProjet</returns>
        public InfosProjet ObtenirInfosProjetFromEssai(int idEssai)
        {
            var proj = resaDB.projet.First(p=>p.id == resaDB.essai.First(e => e.id == idEssai).projetID);

            InfosProjet infos = new InfosProjet()
            {
                DateCreation = proj.date_creation,
                Description = proj.description_projet,
                Financement = proj.financement,
                MailRespProj = proj.mailRespProjet,
                MailUsrSaisie = resaDB.Users.First(p => p.Id == proj.compte_userID).Email,
                NumProjet = proj.num_projet,
                Organisme = resaDB.organisme.First(o => o.id == proj.organismeID).nom_organisme,
                Provenance = proj.provenance,
                TitreProjet = proj.titre_projet,
                TypeProjet = proj.type_projet
            };
            return infos;
        }

        /// <summary>
        /// Validation d'un essai sur la BDD
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns> true ou false</returns>
        public bool ValiderEssai(int idEssai)
        {
            bool changeIsOk = false;
            int retry = 0;

            var essai = resaDB.essai.First(e => e.id == idEssai);
            essai.date_validation = DateTime.Now;
            essai.status_essai = EnumStatusEssai.Validate.ToString();
            while(retry < 3 && changeIsOk != true)
            {
                try
                {
                    resaDB.SaveChanges();
                    changeIsOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Erreur lors de la validation de l'essai N° :" + idEssai);
                    retry++;
                }
            }
            return changeIsOk;           
        }

        /// <summary>
        /// Refuser un essai  sur la BDD
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <param name="raisonRefus">raison du refus saisie par l'administrateur</param>
        /// <returns> true ou false</returns>
        public bool RefuserEssai(int idEssai, string raisonRefus)
        {
            bool changeIsOk = false;
            int retry = 0;

            var essai = resaDB.essai.First(e => e.id == idEssai);
            essai.status_essai = EnumStatusEssai.Refuse.ToString();
            essai.date_validation = DateTime.Now;
            essai.resa_refuse = true;
            essai.raison_refus = raisonRefus;
            while (retry < 3 && changeIsOk != true)
            {
                try
                {
                    resaDB.SaveChanges();
                    changeIsOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Erreur lors de la validation de l'essai N° :" + idEssai);
                    retry++;
                }
            }
            return changeIsOk;
        }

        /// <summary>
        /// Récupérer un "essai" 
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns>essai</returns>
        public essai ObtenirEssai(int idEssai)
        {
            return resaDB.essai.First(e => e.id == idEssai);
        }       
    }
}
