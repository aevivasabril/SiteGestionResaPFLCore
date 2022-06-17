using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats
{
    public class ConsultStatsVM
    {
        public int QuantiteLaitAnnee { get; set; }
        public int QuantiteLaitPeriode { get; set; }

        public List<ZoneStats> ListZones { get; set; }
        public int AnneeActuel { get; set; }

        /// <summary>
        /// Date debut à afficher pour le planning PFL 
        /// </summary>
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateDu { get; set; }

        /// <summary>
        /// Date fin à afficher pour le planning PFL 
        /// </summary>
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateAu { get; set; }

        public List<ld_provenance> ListProvenances { get; set; }

        public List<ld_type_projet> ListTypeProj { get; set; }
    }
}
