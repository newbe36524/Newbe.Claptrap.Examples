namespace Newbe.Claptrap.Ticketing.Models.Seat.Events
{
    public class TakeSeatEvent : IEventData
    {
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public string RequestId { get; set; }
    }
}