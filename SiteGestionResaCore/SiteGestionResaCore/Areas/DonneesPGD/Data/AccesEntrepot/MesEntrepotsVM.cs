using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot
{
    public class MesEntrepotsVM
    {
        public List<EntrepotsXProjet> ListEntrepotXProjet { get; set; }
        public string HideListeDocsXEssai { get; set; }
        public string NumProjetSelect { get; set; }
        public int IdProjSelect { get; set; }
        public List<EssaiAvecEntrepotxProj> ListEssaiAvecEntrepot { get; set; }
        public List<DocXEssai> ListDocsXEssai { get; set; }
        public string HideListeDocs { get; set; }
        public int IdEssaiXAffichage { get; set; }
        public int IdEssai { get; set; }
        public string TitreEssai { get; set; }
        public double TotalKo { get; set; }
    }
}
