using System.Threading.Tasks;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;

namespace Newbe.Claptrap.Ticketing.Actors.Train.Main.Events
{
    public class UpdateCountEventHandler
        : NormalEventHandler<TrainInfo, UpdateCountEvent>
    {
        public override ValueTask HandleEvent(TrainInfo stateData, UpdateCountEvent eventData,
            IEventContext eventContext)
        {
            var stations = stateData.Stations;
            var indexFrom = 0;
            var indexTo = 0;
            for (int i = 0; i < stations.Count; i++)
            {
                var station = stations[i];
                if (station == eventData.FromStationId)
                {
                    indexFrom = i;
                }

                if (station == eventData.ToStationId)
                {
                    indexTo = i;
                }
            }

            for (var i = 0; i < stations.Count - 1; i++)
            {
                for (var j = i + 1; j < stations.Count; j++)
                {
                    if (i < indexTo && j > indexFrom)
                    {
                        stateData.SeatCount[stations[i]][stations[j]]--;
                    }
                }
            }

            return new ValueTask();
        }
    }
}