using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public class EquipsVsJoursVM
    {
        public List<InfosEquipVsJours> ListeEquipVsJours { get; set; }
        public int IdZone { get; set; }
        public string NomZone { get; set; }
    }
}
