namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class LeftCountItem
    {
        public int FromStationId { get; set; }
        public string FromStationName { get; set; }
        public int ToStationId { get; set; }
        public string ToStationName { get; set; }
        public int LeftCount { get; set; }
    }
}