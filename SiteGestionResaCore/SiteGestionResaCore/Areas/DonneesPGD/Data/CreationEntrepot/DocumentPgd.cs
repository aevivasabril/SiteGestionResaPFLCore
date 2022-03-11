using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class DocumentPgd
    {
        //public int id { get; set; }
        public byte[] contenu_document { get; set; }
        public string nom_document { get; set; }
        public int? equipementID { get; set; }
        //public int essaiID { get; set; }
        public int type_documentID { get; set; }
        //public DateTime date_creation { get; set; }
        public int type_activiteID { get; set; }
    }
}
