namespace Newbe.Claptrap.Ticketing.Actors.Seat.Main
{
    public struct SeatId
    {
        public int TrainId { get; set; }
        public int SeatNumber { get; set; }

        public static SeatId FromSeatId(string seatId)
            => FromSeatId(int.Parse(seatId));

        public static SeatId FromSeatId(int seatId)
        {
            var seatNumber = seatId % 10000;
            var trainId = seatId / 10000;
            var re = new SeatId
            {
                SeatNumber = seatNumber,
                TrainId = trainId
            };
            return re;
        }
    }
}