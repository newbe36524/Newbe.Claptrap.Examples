namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class TakeSeatInput
    {
        /// <summary>
        /// Train Id, range in [2,34] U [102,134]
        /// </summary>
        public int TrainId { get; set; }

        /// <summary>
        /// Seat Id. SeatId is composed of trainId and number.
        /// e.g. trainId is 2 and seat number is 0163, then seatId is 20163.
        /// see http://claptrap.newbe.pro/zh_Hans/3-Sample/1-Newbe-Claptrap-Ticketing/1-Design
        /// </summary>
        public string SeatId { get; set; }

        /// <summary>
        /// From Station Id, range in [10001,10034]
        /// </summary>
        public int FromStationId { get; set; }

        /// <summary>
        /// To Station Id, range in [10001,10034]
        /// </summary>
        public int ToStationId { get; set; }
    }
}