using System;
using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Models.Train
{
    public class TrainInfo : IStateData
    {
        public IReadOnlyList<int> Stations { get; set; }
        public IDictionary<StationTuple, int> SeatCount { get; set; }
    }

    public struct StationTuple
    {
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }

        private sealed class FromLocationIdToLocationIdEqualityComparer : IEqualityComparer<StationTuple>
        {
            public bool Equals(StationTuple x, StationTuple y)
            {
                return x.FromStationId == y.FromStationId && x.ToStationId == y.ToStationId;
            }

            public int GetHashCode(StationTuple obj)
            {
                return HashCode.Combine(obj.FromStationId, obj.ToStationId);
            }
        }

        public static IEqualityComparer<StationTuple> FromLocationIdToLocationIdComparer { get; } =
            new FromLocationIdToLocationIdEqualityComparer();
    }
}