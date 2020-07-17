using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class GetSeatOutput
    {
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
        public string FromStationName { get; set; }
        public string ToStationName { get; set; }
        public IEnumerable<SeatListItem> Items { get; set; }
    }
}