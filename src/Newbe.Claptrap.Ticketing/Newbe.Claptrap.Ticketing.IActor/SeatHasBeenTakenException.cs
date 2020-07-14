using System;

namespace Newbe.Claptrap.Ticketing.IActor
{
    public class SeatHasBeenTakenException : Exception
    {
        public int SeatId { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }

        public SeatHasBeenTakenException(int seatId, int fromStationId, int toStationId)
            : this(
                $"Seat has been taken : {seatId}, fromStationId:{fromStationId}, toStationId:{toStationId}",
                seatId,
                fromStationId,
                toStationId)
        {
            SeatId = seatId;
            FromStationId = fromStationId;
            ToStationId = toStationId;
        }

        public SeatHasBeenTakenException(string message, int seatId, int fromStationId, int toStationId) :
            base(message)
        {
            SeatId = seatId;
            FromStationId = fromStationId;
            ToStationId = toStationId;
        }

        public SeatHasBeenTakenException(string message, Exception innerException, int seatId, int fromStationId,
            int toStationId) : base(message, innerException)
        {
            SeatId = seatId;
            FromStationId = fromStationId;
            ToStationId = toStationId;
        }
    }
}