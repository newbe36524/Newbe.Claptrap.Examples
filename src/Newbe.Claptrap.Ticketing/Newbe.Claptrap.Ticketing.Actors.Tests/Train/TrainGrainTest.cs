using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Newbe.Claptrap.Orleans;
using Newbe.Claptrap.Ticketing.Actors.Train.Main;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests.Train
{
    public class TrainGrainTest
    {
        [Test]
        public async Task GetLeftSeatCount_Ok()
        {
            using var mocker = AutoMock.GetStrict();
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(state);
            var grain = mocker.Create<TrainGran>();
            grain.TrainId = 123;
            var count = await grain.GetLeftSeatCountAsync(1000, 1001);
            count.Should().Be(maxCount);
        }

        [Test]
        public void GetLeftSeatCount_StationNotFound()
        {
            using var mocker = AutoMock.GetStrict();
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(state);
            var grain = mocker.Create<TrainGran>();
            grain.TrainId = 123;
            Assert.ThrowsAsync<StationNotFoundException>(() => grain.GetLeftSeatCountAsync(1000, 10000));
        }

        [Test]
        public void UpdateCount_StationNotFound()
        {
            using var mocker = AutoMock.GetStrict();
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(state);
            var grain = mocker.Create<TrainGran>();
            grain.TrainId = 123;
            Assert.ThrowsAsync<StationNotFoundException>(() => grain.UpdateCountAsync(1000, 10000));
        }

        [Test]
        public async Task UpdateCount_Ok()
        {
            using var mocker = AutoMock.GetStrict();
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(state);
            mocker.MockEventHandling<UpdateCountEvent>(ClaptrapCodes.UpdateCount);
            var grain = mocker.Create<TrainGran>();
            grain.TrainId = 123;
            await grain.UpdateCountAsync(1000, 1001);
        }
    }
}