using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class CreationEntrepotVM
    {
        public int idEssai { get; set; }
        public string TitreEssai { get; set; }
        public List<ReservationsXEssai> ListReservationsXEssai { get; set; }
        public List<type_document> ListeTypeDoc { get; set; }
        
        public string NomActivite { get; set; }
        public int IDActivite { get; set; }

        public List<DocAjoutePartieUn> ListDocsPartieUn { get; set; }
        public List<DocAjoutePartieDeux> ListDocsPartieDeux { get; set; }
        public List<DocAjoutePartieUn> ListDocsProduit { get; set; }
        public List<DocAjoutePartieUn> ListDocsProcede { get; set; }
        
        /// <summary>
        /// Id d'un item de la liste des responsables projet
        /// </summary>
        [Required]
        [Display(Name = "Type document*")]
        [Range(1, 20, ErrorMessage = "Sélectionnez un type de document")]
        public int TypeDocumentID { get; set; }

        public IEnumerable<SelectListItem> TypeDocumentItem { get; set; } // Liste pour sélectionner le type de document

        /// <summary>
        /// Id d'un item de la liste des responsables projet
        /// </summary>
        [Required]
        [Display(Name = "Type activité*")]
        [Range(1, 20, ErrorMessage = "Sélectionnez un type d'activité")]
        public int TypeActiviteID { get; set; }

        public IEnumerable<SelectListItem> TypeActiviteItem { get; set; } // Liste pour sélectionner le type de document

        public string NomEquipement { get; set; }
        public int IDEquipement { get; set; }
        public int idResa { get; set; }
        public string NomBoutonPage { get; set; }
        public bool AjoutDocs { get; set; }
        public bool AjoutDocsAdmin { get; set; }

    }
}
