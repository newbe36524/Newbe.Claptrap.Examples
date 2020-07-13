namespace Newbe.Claptrap.Ticketing.Models
{
    public static class ClaptrapCodes
    {
        public const string SeatGrain = "seat_claptrap_newbe";
        private const string SeatEventSuffix = "_e_" + SeatGrain;
        public const string TakeSeat = "takeSeat" + SeatEventSuffix;

        public const string TrainGrain = "seat_claptrap_newbe";
        private const string TrainEventSuffix = "_e_" + TrainGrain;
        public const string UpdateCount = "updateCount" + TrainEventSuffix;
    }
}