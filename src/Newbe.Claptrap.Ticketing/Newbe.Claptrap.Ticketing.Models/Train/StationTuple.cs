using System;
using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Models.Train
{
    public class StationTuple
    {
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }

        private sealed class FromStationIdToStationIdEqualityComparer : IEqualityComparer<StationTuple>
        {
            public bool Equals(StationTuple x, StationTuple y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.FromStationId == y.FromStationId && x.ToStationId == y.ToStationId;
            }

            public int GetHashCode(StationTuple obj)
            {
                return HashCode.Combine(obj.FromStationId, obj.ToStationId);
            }
        }

        public static IEqualityComparer<StationTuple> FromStationIdToStationIdComparer { get; } = new FromStationIdToStationIdEqualityComparer();
    }
}