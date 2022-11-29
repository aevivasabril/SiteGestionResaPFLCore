using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data
{
    public class DocumentMetrologie
    {
        public int idDoc { get; set; }
        public string NomDocument { get; set; }
        public byte[] ContenuDoc { get; set; }
        public DateTime dateAjout { get; set; }
        public string DescriptionDoc { get; set; }
    }
}
