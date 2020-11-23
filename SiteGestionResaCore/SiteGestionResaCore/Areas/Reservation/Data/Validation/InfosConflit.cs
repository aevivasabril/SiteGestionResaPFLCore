namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    // class contenant les infos sur une autre réservation generant un conflit ( pour les projet RESTREINT)
    public class InfosConflit
    {
        public string NomEquipement { get; set; }
        public string ZoneEquipement { get; set; }
        public string MailResponsablePrj { get; set; }
        public string NumProjet { get; set; }
    }
}