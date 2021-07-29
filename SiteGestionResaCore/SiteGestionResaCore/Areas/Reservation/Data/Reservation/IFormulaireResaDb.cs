﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public interface IFormulaireResaDb
    {
        List<ld_type_projet> ObtenirList_TypeProjet();

        List<ld_financement> ObtenirList_Financement();

        List<organisme> ObtenirListOrg();

        List<utilisateur> ObtenirList_UtilisateurValide();

        List<ld_provenance_produit> ObtenirList_ProvenanceProduit();

        List<ld_provenance> ObtenirList_ProvenanceProjet();

        List<ld_produit_in> ObtenirList_TypeProduitEntree();

        List<ld_destination> ObtenirList_DestinationPro();

        Task<IList<utilisateur>> ObtenirLogisticUsersAsync();

        Task<IList<utilisateur>> ObtenirMainAdmUsersAsync();

        List<ld_equipes_stlo> ObtenirListEquips();

    }
}
