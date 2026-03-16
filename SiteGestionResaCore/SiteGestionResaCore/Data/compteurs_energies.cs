using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public partial class compteurs_energies
    {
        public int id { get; set; }
        public string nom_compteur { get; set; }
        public string nomTabPcVue { get; set; }
        public int? equipementID { get; set; }

        public virtual equipement equipement { get; set; }
    }
}
