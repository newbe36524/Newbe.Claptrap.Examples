namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class SeatListItem
    {
        public int TrainId { get; set; }
        public int LeftCount { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
        public string FromStationName { get; set; }
        public string ToStationName { get; set; }
    }
}