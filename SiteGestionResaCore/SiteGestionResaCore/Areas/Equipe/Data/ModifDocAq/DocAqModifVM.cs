using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq
{
    public class DocAqModifVM
    {
        public List<DocQualiteToModif> ListDocToModif { get; set; }

        [Required(ErrorMessage = "Le nom de la rubrique pour le document est requis")]
        public string NomRubriqueDoc { get; set; }
        public int IdDocToSupp { get; set; }
    }
}
