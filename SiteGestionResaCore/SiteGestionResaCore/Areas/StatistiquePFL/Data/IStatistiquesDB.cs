﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public interface IStatistiquesDB
    {
        List<InfosReservations> ObtenirResasDuAu(DateTime datedu, DateTime dateau);
    }
}
