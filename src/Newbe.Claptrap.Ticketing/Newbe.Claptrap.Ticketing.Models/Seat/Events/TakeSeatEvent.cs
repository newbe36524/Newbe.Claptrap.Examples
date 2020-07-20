namespace Newbe.Claptrap.Ticketing.Models.Seat.Events
{
    public class TakeSeatEvent : IEventData
    {
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
        public string RequestId { get; set; }
    }
}