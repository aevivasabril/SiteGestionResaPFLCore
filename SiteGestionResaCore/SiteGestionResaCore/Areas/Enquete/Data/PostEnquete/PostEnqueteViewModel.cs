using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data.PostEnquete
{
    public class PostEnqueteViewModel
    {
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
    }
}
