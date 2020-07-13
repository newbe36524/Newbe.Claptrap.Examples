using System;

namespace Newbe.Claptrap.Ticketing.IActor
{
    public class StationNotFoundException : Exception
    {
        public int TrainId { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }

        public StationNotFoundException(int trainId, int fromStationId, int toStationId)
            : this(
                $"There is no seat found with seatId : {trainId}, fromLocationId:{fromStationId}, toLocationId:{toStationId}",
                trainId,
                fromStationId,
                toStationId)
        {
            this.TrainId = trainId;
            FromStationId = fromStationId;
            ToStationId = toStationId;
        }

        public StationNotFoundException(string message, int trainId, int fromStationId, int toStationId) :
            base(message)
        {
            TrainId = trainId;
            FromStationId = fromStationId;
            ToStationId = toStationId;
        }

        public StationNotFoundException(string message, Exception innerException, int trainId, int fromStationId,
            int toStationId) : base(message, innerException)
        {
            TrainId = trainId;
            FromStationId = fromStationId;
            ToStationId = toStationId;
        }
    }
}