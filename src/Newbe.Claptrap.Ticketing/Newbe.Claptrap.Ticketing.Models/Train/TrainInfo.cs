using System.Collections.Generic;

namespace Newbe.Claptrap.Ticketing.Models.Train
{
    public class TrainInfo : IStateData
    {
        public IReadOnlyList<int> Stations { get; set; }
        public IDictionary<int, IDictionary<int, int>> SeatCount { get; set; }

        public int GetSeatCount(int fromStationId, int toStationId)
        {
            var re = SeatCount[fromStationId][toStationId];
            return re;
        }

        public bool TryGetSeatCount(int fromStationId, int toStationId, out int count)
        {
            if (!SeatCount.TryGetValue(fromStationId, out var toStationDic))
            {
                count = 0;
                return false;
            }

            var re = toStationDic.TryGetValue(toStationId, out count);
            return re;
        }

        public static TrainInfo Create(IReadOnlyList<int> stations, int seatCount)
        {
            var dic = new Dictionary<int, IDictionary<int, int>>();
            for (var i = 0; i < stations.Count - 1; i++)
            {
                for (var j = i + 1; j < stations.Count; j++)
                {
                    var fromStationId = stations[i];
                    var toStationId = stations[j];
                    if (!dic.TryGetValue(fromStationId, out var toDic))
                    {
                        toDic = new Dictionary<int, int>();
                    }

                    toDic[toStationId] = seatCount;
                    dic[fromStationId] = toDic;
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