namespace Newbe.Claptrap.Ticketing.Models
{
    public static class ClaptrapCodes
    {
        public const string SeatGrain = "cart_claptrap_newbe";
        private const string SeatEventSuffix = "_e_" + SeatGrain;
        public const string TakeSeat = "requireSeat" + SeatEventSuffix;
    }
}