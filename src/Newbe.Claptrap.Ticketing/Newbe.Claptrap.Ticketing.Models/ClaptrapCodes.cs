namespace Newbe.Claptrap.Ticketing.Models
{
    public static class ClaptrapCodes
    {
        public const string SeatActor = "seat_claptrap_newbe";
        private const string SeatEventSuffix = "_e_" + SeatActor;
        public const string TakeSeatEvent = "takeSeat" + SeatEventSuffix;
        private const string SeatMinionSuffix = "_m_" + SeatActor;
        public const string SeatUpdateCountMinionActor = "updateCount" + SeatMinionSuffix;

        public const string TrainActor = "train_claptrap_newbe";
        private const string TrainEventSuffix = "_e_" + TrainActor;
        public const string UpdateCount = "updateCount" + TrainEventSuffix;
    }
}