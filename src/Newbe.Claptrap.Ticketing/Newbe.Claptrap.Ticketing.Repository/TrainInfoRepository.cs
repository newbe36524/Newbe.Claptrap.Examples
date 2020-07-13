using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public class TrainInfoRepository : ITrainInfoRepository
    {
        public Task<int[]> GetStationsAsync(int trainId)
        {
            var re = DataSource.TrainLocations[trainId];
            return Task.FromResult(re);
        }

        public Task<int[]> GetTrainsAsync(int locationId)
        {
            var trainIds = DataSource.LocationTrains[locationId];
            return Task.FromResult(trainIds);
        }

        public Task<int[]> GetTrainsAsync(int fromLocationId, int toLocationId)
        {
            var fromTrains = DataSource.LocationTrains[fromLocationId];
            var toTrains = DataSource.LocationTrains[toLocationId];
            var trainIds = fromTrains.Concat(toTrains).ToHashSet();
            var re = DataSource.TrainLocations
                .Where(kv => trainIds.Contains(kv.Key))
                .Where(kv =>
                {
                    var locations = kv.Value;
                    var matchFrom = false;
                    var matchTo = false;
                    foreach (var location in locations)
                    {
                        if (!matchFrom && location == fromLocationId)
                        {
                            matchFrom = true;
                        }

                        if (matchFrom && location == toLocationId)
                        {
                            matchTo = true;
                            break;
                        }
                    }

                    return matchTo;
                })
                .Select(x => x.Key)
                .ToArray();
            return Task.FromResult(re);
        }
    }
}