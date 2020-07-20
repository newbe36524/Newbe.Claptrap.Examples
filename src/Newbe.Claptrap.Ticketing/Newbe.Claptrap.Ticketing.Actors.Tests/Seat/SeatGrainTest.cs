using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Actors.Seat.Main;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests.Seat
{
    public class SeatGrainTest
    {
        [Test]
        public async Task Normal()
        {
            using var mocker = AutoMock.GetStrict();
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(seatInfo);
            mocker.MockEventHandling<TakeSeatEvent>(ClaptrapCodes.TakeSeatEvent);

            var grain = mocker.Create<SeatGrain>();
            grain.SeatId = 123;
            var reqId1 = "newbe36524-1";
            await grain.TakeSeatAsync(1000, 1001, reqId1);
        }

        [Test]
        public void StationNotFound()
        {
            using var mocker = AutoMock.GetStrict();
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(seatInfo);
            var grain = mocker.Create<SeatGrain>();
            grain.SeatId = 123;
            const string reqId1 = "newbe36524-1";
            Assert.ThrowsAsync<StationNotFoundException>(()
                => grain.TakeSeatAsync(1000, 9999, reqId1));
            Assert.ThrowsAsync<StationNotFoundException>(()
                => grain.TakeSeatAsync(1, 1000, reqId1));
        }

        [Test]
        public void StationMismatched()
        {
            using var mocker = AutoMock.GetStrict();
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(seatInfo);
            var grain = mocker.Create<SeatGrain>();
            grain.SeatId = 123;
            const string reqId1 = "newbe36524-1";
            Assert.ThrowsAsync<StationNotFoundException>(()
                => grain.TakeSeatAsync(1001, 1000, reqId1));
        }

        [Test]
        public void AlreadyTaken()
        {
            using var mocker = AutoMock.GetStrict();
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            seatInfo.RequestIds[0] = "taken";
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(seatInfo);
            var grain = mocker.Create<SeatGrain>();
            grain.SeatId = 123;
            const string reqId1 = "newbe36524-1";
            Assert.ThrowsAsync<SeatHasBeenTakenException>(()
                => grain.TakeSeatAsync(1000, 1002, reqId1));
        }
    }
}