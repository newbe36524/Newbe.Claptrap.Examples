using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Newbe.Claptrap.Ticketing.Actors.Seat.Events;
using Newbe.Claptrap.Ticketing.Models.Seat;
using Newbe.Claptrap.Ticketing.Models.Seat.Events;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests.Seat.Events
{
    public class TakeSeatEventHandlerTest
    {
        [Test]
        public async Task TakeOne()
        {
            using var mocker = AutoMock.GetStrict();
            await using var handler = mocker.Create<TakeSeatEventHandler>();
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            var reqId1 = "newbe36524-1";
            var takeSeatEvent = new TakeSeatEvent
            {
                RequestId = reqId1,
                FromLocationId = 1000,
                ToLocationId = 1001
            };
            await handler.HandleEvent(seatInfo, takeSeatEvent, default);
            seatInfo.RequestIds.Should().BeEquivalentTo(reqId1, string.Empty, string.Empty);
        }

        [Test]
        public async Task TakeMore()
        {
            using var mocker = AutoMock.GetStrict();
            await using var handler = mocker.Create<TakeSeatEventHandler>();
            var seatInfo = SeatInfo.Create(Enumerable.Range(1000, 4).ToList());
            var reqId1 = "newbe36524-1";
            var takeSeatEvent = new TakeSeatEvent
            {
                RequestId = reqId1,
                FromLocationId = 1000,
                ToLocationId = 1002
            };
            await handler.HandleEvent(seatInfo, takeSeatEvent, default);
            seatInfo.RequestIds.Should().BeEquivalentTo(reqId1, reqId1, string.Empty);
        }
    }
}