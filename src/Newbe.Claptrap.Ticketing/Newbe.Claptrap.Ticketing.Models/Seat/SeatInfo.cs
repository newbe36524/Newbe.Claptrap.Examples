using System.Collections.Generic;
using System.Linq;

namespace Newbe.Claptrap.Ticketing.Models.Seat
{
    public class SeatInfo : IStateData
    {
        /// <summary>
        /// station ids.
        /// </summary>
        public IReadOnlyList<int> Stations { get; set; }

        /// <summary>
        ///  station ids dictionary.
        /// {key:stationId, value:index of <see cref="Stations"/>}
        /// </summary>
        public IDictionary<int, int> StationDic { get; set; }

        /// <summary>
        /// request ids.
        /// data in index 0 means that journey interval from station 0 to station 1 is taken by which request id. it is string.Empty if no one take it.
        /// </summary>
        public IList<string> RequestIds { get; set; }

        public static SeatInfo Create(IReadOnlyList<int> stations)
        {
            var re = new SeatInfo
            {
                Stations = stations,
                StationDic = stations
                    .Select((station, index) => (station, index))
                    .ToDictionary(x => x.station, x => x.index),
                RequestIds = stations.Take(stations.Count - 1)
                    .Select((x, i) => string.Empty)
                    .ToList()
            };
            return re;
        }
    }
}