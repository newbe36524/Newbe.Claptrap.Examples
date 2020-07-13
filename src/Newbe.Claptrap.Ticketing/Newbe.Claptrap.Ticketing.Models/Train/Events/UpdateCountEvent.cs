namespace Newbe.Claptrap.Ticketing.Models.Train.Events
{
    public class UpdateCountEvent : IEventData
    {
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
    }
}