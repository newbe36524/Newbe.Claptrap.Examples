using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Newbe.Claptrap.Ticketing.Actors.Train.Main.Events;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests.Train.Events
{
    public class UpdateCountEventHandlerTest
    {
        [Test]
        public async Task UpdateOne()
        {
            using var mocker = AutoMock.GetStrict();
            await using var handler = mocker.Create<UpdateCountEventHandler>();
            var stations = Enumerable.Range(100, 4).ToArray();
            var maxCount = 10000;
            var state = TrainInfo.Create(stations, maxCount);
            var evt = new UpdateCountEvent
            {
                FromStationId = 100,
                ToStationId = 101
            };
            await handler.HandleEvent(state, evt, default);
            var leftCount = maxCount - 1;
            state.SeatCount.Count.Should().Be(6);
            state.GetSeatCount(100, 101).Should().Be(leftCount);
            state.GetSeatCount(100, 102).Should().Be(leftCount);
            state.GetSeatCount(100, 103).Should().Be(leftCount);
            state.GetSeatCount(101, 102).Should().Be(maxCount);
            state.GetSeatCount(101, 103).Should().Be(maxCount);
            state.GetSeatCount(102, 103).Should().Be(maxCount);
        }

        [Test]
        public async Task UpdateTwice()
        {
            using var mocker = AutoMock.GetStrict();
            await using var handler = mocker.Create<UpdateCountEventHandler>();
            var stations = Enumerable.Range(100, 4).ToArray();
            const int maxCount = 10000;
            var state = TrainInfo.Create(stations, maxCount);

            await handler.HandleEvent(state, new UpdateCountEvent
            {
                FromStationId = 100,
                ToStationId = 101
            }, default);

            await handler.HandleEvent(state, new UpdateCountEvent
            {
                FromStationId = 102,
                ToStationId = 103
            }, default);

            state.SeatCount.Count.Should().Be(6);
            state.GetSeatCount(100, 101).Should().Be(maxCount - 1);
            state.GetSeatCount(100, 102).Should().Be(maxCount - 1);
            state.GetSeatCount(100, 103).Should().Be(maxCount - 2);
            state.GetSeatCount(101, 102).Should().Be(maxCount);
            state.GetSeatCount(101, 103).Should().Be(maxCount - 1);
            state.GetSeatCount(102, 103).Should().Be(maxCount - 1);
        }

        [Test]
        public async Task UpdateMoreThanOneStep()
        {
            using var mocker = AutoMock.GetStrict();
            await using var handler = mocker.Create<UpdateCountEventHandler>();
            var stations = Enumerable.Range(100, 4).ToArray();
            const int maxCount = 10000;
            var state = TrainInfo.Create(stations, maxCount);

            var wvt = new UpdateCountEvent
            {
                FromStationId = 100,
                ToStationId = 103
            };
            await handler.HandleEvent(state, wvt, default);

            state.SeatCount.Count.Should().Be(6);
            var leftCount = maxCount - 1;
            state.GetSeatCount(100, 101).Should().Be(leftCount);
            state.GetSeatCount(100, 102).Should().Be(leftCount);
            state.GetSeatCount(100, 103).Should().Be(leftCount);
            state.GetSeatCount(101, 102).Should().Be(leftCount);
            state.GetSeatCount(101, 103).Should().Be(leftCount);
            state.GetSeatCount(102, 103).Should().Be(leftCount);
        }
    }
}