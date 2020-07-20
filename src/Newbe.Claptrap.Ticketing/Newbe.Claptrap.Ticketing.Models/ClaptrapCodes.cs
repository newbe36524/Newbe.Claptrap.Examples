namespace Newbe.Claptrap.Ticketing.Models
{
    public static class ClaptrapCodes
    {
        public const string SeatGrain = "seat_claptrap_newbe";
        private const string SeatEventSuffix = "_e_" + SeatGrain;
        public const string TakeSeatEvent = "takeSeat" + SeatEventSuffix;
        private const string SeatMinionSuffix = "_m_" + SeatGrain;
        public const string SeatUpdateCountMinionGrain = "updateCount" + SeatMinionSuffix;

        public const string TrainGrain = "train_claptrap_newbe";
        private const string TrainEventSuffix = "_e_" + TrainGrain;
        public const string UpdateCount = "updateCount" + TrainEventSuffix;
    }
}