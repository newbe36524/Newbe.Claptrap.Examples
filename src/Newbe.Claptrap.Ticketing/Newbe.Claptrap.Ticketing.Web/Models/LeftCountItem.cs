namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class LeftCountItem
    {
        /// <summary>
        /// From Station Id
        /// </summary>
        public int FromStationId { get; set; }

        /// <summary>
        /// To Station Id
        /// </summary>
        public int ToStationId { get; set; }

        /// <summary>
        /// From Station Name
        /// </summary>
        public string FromStationName { get; set; }

        /// <summary>
        /// To Station Name
        /// </summary>
        public string ToStationName { get; set; }

        /// <summary>
        /// Left Seat Count
        /// </summary>
        public int LeftCount { get; set; }
    }
}