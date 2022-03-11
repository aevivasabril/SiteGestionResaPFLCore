﻿using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot
{
    public class AccesEntrepotDB: IAccesEntrepotDB
    {
        private readonly GestionResaContext contextDB;
        private readonly ILogger<AccesEntrepotDB> logger;

        public AccesEntrepotDB(
            GestionResaContext contextDB,
            ILogger<AccesEntrepotDB> logger)
        {
            this.contextDB = contextDB;
            this.logger = logger;
        }
        public List<EntrepotsXProjet> ObtenirListProjetsAvecEntrepotCree(utilisateur user)
        {
            List<EntrepotsXProjet> list = new List<EntrepotsXProjet>();
            var projetXUser = contextDB.projet.Where(p => p.compte_userID == user.Id);
            foreach(var x in projetXUser)
            {

                if(contextDB.essai.Where(e => e.projetID == x.id).ToList().Any(e=>e.entrepot_cree == true))
                {
                    EntrepotsXProjet entre = new EntrepotsXProjet { NomProjet = x.titre_projet, NumProjet = x.num_projet, IdProjet = x.id };
                    list.Add(entre);
                }
            }
            return list;
        }

        public List<EssaiAvecEntrepotxProj> ObtenirListEssaiAvecEntrepot(int IdProjet)
        {
            List<EssaiAvecEntrepotxProj> ListEssai = new List<EssaiAvecEntrepotxProj>();
            var essais = contextDB.essai.Where(e => e.projetID == IdProjet && e.entrepot_cree == true).ToList();
            foreach(var essai in essais)
            {
                EssaiAvecEntrepotxProj EssaiAvecEnt = new EssaiAvecEntrepotxProj { IdEssai = essai.id, TitreEssai = essai.titreEssai };
                ListEssai.Add(EssaiAvecEnt);
            }
            return ListEssai;
        }
        public string ObtNumProjet(int IdProjet)
        {
            return contextDB.projet.First(p => p.id == IdProjet).num_projet;
        }

        public List<DocXEssai> ObtListDocsXEssai(int IdEssai)
        {
            List<DocXEssai> listDocs = new List<DocXEssai>();
            DocXEssai doc = new DocXEssai();
            var list = contextDB.doc_essai_pgd.Where(d => d.essaiID == IdEssai).ToList();
            foreach(var l in list)
            {
                if(l.equipementID != null)
                {
                    doc = new DocXEssai
                    {
                        IdDoc = l.id,
                        IdEquipement = l.equipementID.Value,
                        NomEquipement = contextDB.equipement.FirstOrDefault(e => e.id == l.equipementID).nom,
                        TypeDonnees = contextDB.type_document.First(t => t.id == l.type_documentID).nom_document,
                        TypeActivite = contextDB.activite_pfl.First(a => a.id == l.type_activiteID).nom_activite,
                        NomDocument = l.nom_document
                    };
                }
                else
                {
                    doc = new DocXEssai
                    {
                        IdDoc = l.id,
                        TypeDonnees = contextDB.type_document.First(t => t.id == l.type_documentID).nom_document,
                        TypeActivite = contextDB.activite_pfl.First(a => a.id == l.type_activiteID).nom_activite,
                        NomDocument = l.nom_document
                    };
                    
                }
                
                listDocs.Add(doc);
            }

            return listDocs;
        }

        public string ObtTitreEssai(int IdEssai)
        {
            return contextDB.essai.First(e => e.id == IdEssai).titreEssai;
        }
    }
}
