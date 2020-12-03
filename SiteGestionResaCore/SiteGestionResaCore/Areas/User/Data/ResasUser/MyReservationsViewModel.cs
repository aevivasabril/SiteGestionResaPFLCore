using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data
{
    public class MyReservationsViewModel
    {
        public List<InfosResasUser> ResasUser { get; set; }

        public ResEssaiChildViewModel ChildVmModifEssai { get; set; }
    }
}
