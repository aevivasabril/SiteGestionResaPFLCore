﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SiteGestionResaCore.Data
{
    public partial class utilisateur : IdentityUser<int>
    {
        public utilisateur()
        {
            essai = new HashSet<essai>();
        }

        public string nom { get; set; }
        public string prenom { get; set; }
        public int? organismeID { get; set; }
        public bool? compteInactif { get; set; }

        public virtual organisme organisme { get; set; }
        public virtual ICollection<essai> essai { get; set; }
    }
}