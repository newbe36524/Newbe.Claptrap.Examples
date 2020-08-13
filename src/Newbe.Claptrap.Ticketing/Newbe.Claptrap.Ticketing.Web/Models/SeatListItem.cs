namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class SeatListItem
    {
        /// <summary>
        /// Train Id
        /// </summary>
        public int TrainId { get; set; }

        /// <summary>
        /// Left Count of Seat on the Train
        /// </summary>
        public int LeftCount { get; set; }

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
    }
}