using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.Profil
{
    public interface IProfilDB
    {
        string ObtNomEquipe(int equipeID);
        string ObtNomOrganisme(int orgID);
        List<EnquetesNonReponduesXUsr> ObtListEnquetes(utilisateur usr);
    }
}
