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
            var locations = Enumerable.Range(1000, 4).ToList();
            var ticketing = new Ticketing(locations);
            ticketing.LocationDic.Count.Should().Be(locations.Count);
            ticketing.RequestIds.Count.Should().Be(locations.Count - 1);
            var reqId1 = "newbe36524-1";
            await ticketing.GetSitAsync(1000, 1001, reqId1);
            ticketing.RequestIds.Should().BeEquivalentTo(reqId1, string.Empty, string.Empty);
        }

        [Test]
        public async Task TakeMore()
        {
            var locations = Enumerable.Range(1000, 4).ToList();
            var ticketing = new Ticketing(locations);
            ticketing.LocationDic.Count.Should().Be(locations.Count);
            ticketing.RequestIds.Count.Should().Be(locations.Count - 1);
            var reqId1 = "newbe36524-1";
            await ticketing.GetSitAsync(1000, 1002, reqId1);
            ticketing.RequestIds.Should().BeEquivalentTo(reqId1, reqId1, string.Empty);
        }

        [Test]
        public async Task Conflict()
        {
            var locations = Enumerable.Range(1000, 4).ToList();
            var ticketing = new Ticketing(locations);
            ticketing.LocationDic.Count.Should().Be(locations.Count);
            ticketing.RequestIds.Count.Should().Be(locations.Count - 1);
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
        Task GetSitAsync(int fromLocationId, int toLocationId, string requestId);
    }

    public class Ticketing : ITicketing
    {
        public IList<int> Locations { get; }
        public Dictionary<int, int> LocationDic { get; }
        public List<string> RequestIds { get; }

        public Ticketing(
            IList<int> locations)
        {
            Locations = locations;
            LocationDic = Locations
                .Select((location, index) => (location, index))
                .ToDictionary(x => x.location, x => x.index);
            RequestIds = Locations.Take(Locations.Count - 1)
                .Select((x, i) => string.Empty)
                .ToList();
        }

        public Task GetSitAsync(int fromLocationId, int toLocationId, string requestId)
        {
            var fromIndex = LocationDic[fromLocationId];
            var toIndex = LocationDic[toLocationId];
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