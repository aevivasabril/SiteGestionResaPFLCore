using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.DataPcVue
{
    public class AllDataPcVue
    {
        public List<DataPcVueEquip> DataEquipement { get; set; }

        public string NomEquipement { get; set; }
        public string NumGmao { get; set; }
    }
}
