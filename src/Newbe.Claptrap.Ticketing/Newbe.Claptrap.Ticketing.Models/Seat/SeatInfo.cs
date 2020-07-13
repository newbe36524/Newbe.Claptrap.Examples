using System.Collections.Generic;
using System.Linq;

namespace Newbe.Claptrap.Ticketing.Models.Seat
{
    public class SeatInfo : IStateData
    {
        /// <summary>
        /// location ids.
        /// </summary>
        public IReadOnlyList<int> Locations { get; set; }

        /// <summary>
        ///  location ids dictionary.
        /// {key:locationId, value:index of <see cref="Locations"/>}
        /// </summary>
        public IDictionary<int, int> LocationDic { get; set; }

        /// <summary>
        /// request ids.
        /// data in index 0 means that journey interval from location 0 to location 1 is taken by which request id. it is string.Empty if no one take it.
        /// </summary>
        public IList<string> RequestIds { get; set; }

        public static SeatInfo Create(IReadOnlyList<int> locations)
        {
            var re = new SeatInfo
            {
                Locations = locations,
                LocationDic = locations
                    .Select((location, index) => (location, index))
                    .ToDictionary(x => x.location, x => x.index),
                RequestIds = locations.Take(locations.Count - 1)
                    .Select((x, i) => string.Empty)
                    .ToList()
            };
            return re;
        }
    }
}