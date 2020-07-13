using System;

namespace Newbe.Claptrap.Ticketing.IActor
{
    public class SeatHasBeenTakenException : Exception
    {
        public int SeatId { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }

        public SeatHasBeenTakenException(int seatId, int fromLocationId, int toLocationId)
            : this(
                $"Seat has been taken : {seatId}, fromLocationId:{fromLocationId}, toLocationId:{toLocationId}",
                seatId,
                fromLocationId,
                toLocationId)
        {
            SeatId = seatId;
            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;
        }

        public SeatHasBeenTakenException(string message, int seatId, int fromLocationId, int toLocationId) :
            base(message)
        {
            SeatId = seatId;
            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;
        }

        public SeatHasBeenTakenException(string message, Exception innerException, int seatId, int fromLocationId,
            int toLocationId) : base(message, innerException)
        {
            SeatId = seatId;
            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;
        }
    }
}