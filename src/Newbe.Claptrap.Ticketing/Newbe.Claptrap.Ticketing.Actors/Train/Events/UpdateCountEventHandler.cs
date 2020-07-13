using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;

namespace Newbe.Claptrap.Ticketing.Actors.Train.Events
{
    public class UpdateCountEventHandler
        : NormalEventHandler<TrainInfo, UpdateCountEvent>
    {
        public override ValueTask HandleEvent(TrainInfo stateData, UpdateCountEvent eventData,
            IEventContext eventContext)
        {
            var stations = stateData.Stations;
            var list = new List<int>(stations.Count);
            var matchFrom = false;
            foreach (var station in stations)
            {
                if (station == eventData.FromStationId)
                {
                    list.Add(station);
                    matchFrom = true;
                }

                if (matchFrom)
                {
                    list.Add(station);
                    if (station == eventData.ToStationId)
                    {
                        break;
                    }
                }
            }

            for (var i = 0; i < list.Count; i++)
            {
                for (var j = i; j < list.Count; j++)
                {
                    var key = new StationTuple
                    {
                        FromStationId = stations[i],
                        ToStationId = stations[j]
                    };
                    stateData.SeatCount[key]--;
                }
            }

            return new ValueTask();
        }
    }
}