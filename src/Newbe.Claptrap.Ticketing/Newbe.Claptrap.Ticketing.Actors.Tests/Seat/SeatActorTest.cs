using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Newbe.Claptrap.Dapr.TestKit;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests.Seat
{
    public class SeatActorTest
    {
        [Test]
        public async Task Normal()
        {
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(SeatActor));
            using var mocker = claptrapDesign.CreateAutoMock("123", seatInfo);

            var actor = mocker.Create<SeatActor>();
            var reqId1 = "newbe36524-1";
            await actor.TakeSeatAsync(1000, 1001, reqId1);
        }

        [Test]
        public void StationNotFound()
        {
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(SeatActor));
            using var mocker = claptrapDesign.CreateAutoMock("123", seatInfo);
            var actor = mocker.Create<SeatActor>();
            const string reqId1 = "newbe36524-1";
            Assert.ThrowsAsync<StationNotFoundException>(()
                => actor.TakeSeatAsync(1000, 9999, reqId1));
            Assert.ThrowsAsync<StationNotFoundException>(()
                => actor.TakeSeatAsync(1, 1000, reqId1));
        }

        [Test]
        public void StationMismatched()
        {
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(SeatActor));
            using var mocker = claptrapDesign.CreateAutoMock("123", seatInfo);
            var actor = mocker.Create<SeatActor>();
            const string reqId1 = "newbe36524-1";
            Assert.ThrowsAsync<StationNotFoundException>(()
                => actor.TakeSeatAsync(1001, 1000, reqId1));
        }

        [Test]
        public void AlreadyTaken()
        {
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            seatInfo.RequestIds[0] = "taken";
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(SeatActor));
            using var mocker = claptrapDesign.CreateAutoMock("123", seatInfo);
            var actor = mocker.Create<SeatActor>();
            const string reqId1 = "newbe36524-1";
            Assert.ThrowsAsync<SeatHasBeenTakenException>(()
                => actor.TakeSeatAsync(1000, 1002, reqId1));
        }
    }
}