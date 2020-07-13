using System;

namespace Newbe.Claptrap.Ticketing.IActor
{
    public class LocationNotFoundException : Exception
    {
        public int SeatId { get; set; }
        public int FromLocationId { get; set; }
        public int ToToLocationId { get; set; }

        public LocationNotFoundException(int seatId, int fromLocationId, int toLocationId)
            : this(
                $"There is no seat found with seatId : {seatId}, fromLocationId:{fromLocationId}, toLocationId:{toLocationId}",
                seatId,
                fromLocationId,
                toLocationId)
        {
            SeatId = seatId;
            FromLocationId = fromLocationId;
            ToToLocationId = toLocationId;
        }

        public LocationNotFoundException(string message, int seatId, int fromLocationId, int toLocationId) :
            base(message)
        {
            SeatId = seatId;
            FromLocationId = fromLocationId;
            ToToLocationId = toLocationId;
        }

        public LocationNotFoundException(string message, Exception innerException, int seatId, int fromLocationId,
            int toLocationId) : base(message, innerException)
        {
            SeatId = seatId;
            FromLocationId = fromLocationId;
            ToToLocationId = toLocationId;
        }
    }
}