﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace SiteGestionResaCore.Data
{
    public partial class equipement
    {
        public equipement()
        {
            reservation_projet = new HashSet<reservation_projet>();
        }

        public int id { get; set; }
        public string nom { get; set; }
        public int zoneID { get; set; }
        public string numGmao { get; set; }
        public bool? mobile { get; set; }

        public virtual zone zone { get; set; }
        public virtual ICollection<reservation_projet> reservation_projet { get; set; }
    }
}