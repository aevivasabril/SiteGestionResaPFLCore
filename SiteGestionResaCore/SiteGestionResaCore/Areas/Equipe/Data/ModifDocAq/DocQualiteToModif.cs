using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq
{
    public class DocQualiteToModif
    {
        public int IdDoc { get; set; }
        public string NomDocument { get; set; }
        public string NomRubriqDoc { get; set; }
        public string DescripDoc { get; set; }
        public DateTime DernierDateModif { get; set; }
        public byte[] ContenuDoc { get; set; }
    }
}
