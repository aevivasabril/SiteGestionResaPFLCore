using System;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    // class contenant les infos sur une autre réservation generant un conflit ( pour les projet RESTREINT)
    public class InfosConflit
    {
        // nom "equipement"
        public string NomEquipement { get; set; }
        // nom "zone"
        public string ZoneEquipement { get; set; }
        // mailRespProjet "projet"
        public string MailResponsablePrj { get; set; }
        // num_projet "projet"
        public string NumProjet { get; set; }

        public DateTime DateDeb { get; set; }

        public DateTime DateFin { get; set; }
    }
}