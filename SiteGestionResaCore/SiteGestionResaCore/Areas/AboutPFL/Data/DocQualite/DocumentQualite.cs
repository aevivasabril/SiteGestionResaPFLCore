﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite
{
    public class DocumentQualite
    {
        public int IdDocument { get; set; }
        public string NomDocument { get; set; }
        public string CheminDocument { get; set; }
        public string DescriptionDoc { get; set; }
    }
}
