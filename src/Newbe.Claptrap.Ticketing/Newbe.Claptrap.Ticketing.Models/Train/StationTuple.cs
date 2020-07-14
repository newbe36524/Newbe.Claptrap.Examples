using System;
using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Models.Train
{
    public struct StationTuple
    {
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }

        private sealed class FromStationIdToStationIdEqualityComparer : IEqualityComparer<StationTuple>
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

        public static IEqualityComparer<StationTuple> FromStationIdToStationIdComparer { get; } =
            new FromStationIdToStationIdEqualityComparer();

        public override string ToString()
        {
            return $"{nameof(FromStationId)}: {FromStationId}, {nameof(ToStationId)}: {ToStationId}";
        }
    }
}