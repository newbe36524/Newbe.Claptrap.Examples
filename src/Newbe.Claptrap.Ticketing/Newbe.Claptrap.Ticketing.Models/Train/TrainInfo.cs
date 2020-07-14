using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Models.Train
{
    public class TrainInfo : IStateData
    {
        public IReadOnlyList<int> Stations { get; set; }
        public IDictionary<StationTuple, int> SeatCount { get; set; }

        public int GetSeatCount(int fromStationId, int toStationId)
        {
            var key = new StationTuple
            {
                FromStationId = fromStationId,
                ToStationId = toStationId
            };
            var re = SeatCount[key];
            return re;
        }

        public bool TryGetSeatCount(int fromStationId, int toStationId, out int count)
        {
            var key = new StationTuple
            {
                FromStationId = fromStationId,
                ToStationId = toStationId
            };
            return SeatCount.TryGetValue(key, out count);
        }

        public static TrainInfo Create(IReadOnlyList<int> stations, int seatCount)
        {
            var dic = new Dictionary<StationTuple, int>(StationTuple.FromStationIdToStationIdComparer);
            for (var i = 0; i < stations.Count - 1; i++)
            {
                for (var j = i + 1; j < stations.Count; j++)
                {
                    var stationTuple = new StationTuple
                    {
                        FromStationId = stations[i],
                        ToStationId = stations[j]
                    };
                    dic[stationTuple] = seatCount;
                }
            }

            var re = new TrainInfo
            {
                Stations = stations,
                SeatCount = dic
            };
            return re;
        }
    }
}