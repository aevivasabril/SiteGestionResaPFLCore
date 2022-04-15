using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip
{
    public interface IEquipsToModifDB
    {
        List<InfosEquipement> ListeEquipementsXZone(int idZone);
        bool AjouterFicheXEquipement(int idEquipement, byte[] content, string nom);
        bool SupprimerFicheMat(int IdFiche);
        bool ModifierFicheMat(int IdFiche, byte[] Contenu, string nomDoc);
        string ObtNomEquipement(int IdEquipement);
        bool ModifierNumGMAO(string numGmao, int IdEquipement);
    }
}
