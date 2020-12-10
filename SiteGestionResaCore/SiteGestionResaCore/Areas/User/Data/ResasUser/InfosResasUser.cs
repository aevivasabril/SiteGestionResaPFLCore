using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public class InfosResasUser
    {
        public int IdEssai { get; set; }
        public string CommentEssai { get; set; }
        public DateTime DateCreation { get; set; }
        public string StatusEssai { get; set; }
        public string OpenPartialEssai { get; set; }
        public string OpenReservations { get; set; }

        public string NumProjet { get; set; }
        public string TitreProj { get; set; }
        
    }
}
