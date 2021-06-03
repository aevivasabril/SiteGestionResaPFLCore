using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data.PostEnquete
{
    public interface IPostEnqueteDB
    {
        List<enquete> GetReponsesEnquetes(DateTime datedu, DateTime dateau);
        essai GetEssai(int IdEssai);
        projet GetProjet(int IdProj);
    }
}
