namespace Newbe.Claptrap.Ticketing.Actors.Seat
{
    public struct SeatId
    {
        public int TrainId { get; set; }
        public int SeatNumber { get; set; }

        public static SeatId FromSeatId(string seatId)
        {
            var i = int.Parse(seatId);
            var seatNumber = i % 10000;
            var trainId = i / 10000;
            var re = new SeatId
            {
                SeatNumber = seatNumber,
                TrainId = trainId
            };
            return re;
        }
    }
}