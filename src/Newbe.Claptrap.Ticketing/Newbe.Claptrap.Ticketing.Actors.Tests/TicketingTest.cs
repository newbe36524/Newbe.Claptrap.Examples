using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests
{
    public class TicketingTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TakeOne()
        {
            var stations = Enumerable.Range(1000, 4).ToList();
            var ticketing = new Ticketing(stations);
            ticketing.StationDic.Count.Should().Be(stations.Count);
            ticketing.RequestIds.Count.Should().Be(stations.Count - 1);
            var reqId1 = "newbe36524-1";
            await ticketing.GetSitAsync(1000, 1001, reqId1);
            ticketing.RequestIds.Should().BeEquivalentTo(reqId1, string.Empty, string.Empty);
        }

        [Test]
        public async Task TakeMore()
        {
            var stations = Enumerable.Range(1000, 4).ToList();
            var ticketing = new Ticketing(stations);
            ticketing.StationDic.Count.Should().Be(stations.Count);
            ticketing.RequestIds.Count.Should().Be(stations.Count - 1);
            var reqId1 = "newbe36524-1";
            await ticketing.GetSitAsync(1000, 1002, reqId1);
            ticketing.RequestIds.Should().BeEquivalentTo(reqId1, reqId1, string.Empty);
        }

        [Test]
        public async Task Conflict()
        {
            var stations = Enumerable.Range(1000, 4).ToList();
            var ticketing = new Ticketing(stations);
            ticketing.StationDic.Count.Should().Be(stations.Count);
            ticketing.RequestIds.Count.Should().Be(stations.Count - 1);
            var reqId1 = "newbe36524-1";
            await ticketing.GetSitAsync(1000, 1001, reqId1);
            ticketing.RequestIds.Should().BeEquivalentTo(reqId1, string.Empty, string.Empty);

            var reqId2 = "newbe36524-2";
            await ticketing.GetSitAsync(1000, 1002, reqId2);
            ticketing.RequestIds.Should().BeEquivalentTo(reqId1, string.Empty, string.Empty);
        }
    }

    public interface ITicketing
    {
        Task GetSitAsync(int fromStationId, int toStationId, string requestId);
    }

    public class Ticketing : ITicketing
    {
        public IList<int> Stations { get; }
        public Dictionary<int, int> StationDic { get; }
        public List<string> RequestIds { get; }

        public Ticketing(
            IList<int> stations)
        {
            Stations = stations;
            StationDic = Stations
                .Select((station, index) => (station, index))
                .ToDictionary(x => x.station, x => x.index);
            RequestIds = Stations.Take(Stations.Count - 1)
                .Select((x, i) => string.Empty)
                .ToList();
        }

        public Task GetSitAsync(int fromStationId, int toStationId, string requestId)
        {
            var fromIndex = StationDic[fromStationId];
            var toIndex = StationDic[toStationId];
            var distance = toIndex - fromIndex;
            var notRequested = RequestIds
                .Skip(fromIndex)
                .Take(distance)
                .All(string.IsNullOrEmpty);
            if (notRequested)
            {
                for (var i = fromIndex; i < toIndex; i++)
                {
                    RequestIds[i] = requestId;
                }
            }

            return Task.CompletedTask;
        }
    }
}