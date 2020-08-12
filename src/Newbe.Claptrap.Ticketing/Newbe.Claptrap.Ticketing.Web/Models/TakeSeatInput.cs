namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class TakeSeatInput
    {
        public int TrainId { get; set; }
        public string SeatId { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
    }
}