using System;
using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Models.Train
{
    public class TrainInfo
    {
        public Dictionary<LocationTuple, int> SeatCount { get; set; }
    }

    public struct LocationTuple
    {
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }

        private sealed class FromLocationIdToLocationIdEqualityComparer : IEqualityComparer<LocationTuple>
        {
            public bool Equals(LocationTuple x, LocationTuple y)
            {
                return x.FromLocationId == y.FromLocationId && x.ToLocationId == y.ToLocationId;
            }

            public int GetHashCode(LocationTuple obj)
            {
                return HashCode.Combine(obj.FromLocationId, obj.ToLocationId);
            }
        }

        public static IEqualityComparer<LocationTuple> FromLocationIdToLocationIdComparer { get; } =
            new FromLocationIdToLocationIdEqualityComparer();
    }
}