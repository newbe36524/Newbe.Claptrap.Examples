using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Newbe.Claptrap.Dapr.TestKit;
using Newbe.Claptrap.Ticketing.Actors.Train.Main;
using Newbe.Claptrap.Ticketing.IActor;
using Newbe.Claptrap.Ticketing.Models;
using Newbe.Claptrap.Ticketing.Models.Train;
using Newbe.Claptrap.Ticketing.Models.Train.Events;
using NUnit.Framework;

namespace Newbe.Claptrap.Ticketing.Actors.Tests.Train
{
    public class TrainActorTest
    {
        [Test]
        public async Task GetLeftSeatCount_Ok()
        {
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(TrainGran));
            using var mocker = claptrapDesign.CreateAutoMock("123", state);
            var actor = mocker.Create<TrainGran>();
            var count = await actor.GetLeftSeatCountAsync(1000, 1001);
            count.Should().Be(maxCount);
        }

        [Test]
        public void GetLeftSeatCount_StationNotFound()
        {
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(TrainGran));
            using var mocker = claptrapDesign.CreateAutoMock("123", state);
            var actor = mocker.Create<TrainGran>();
            Assert.ThrowsAsync<StationNotFoundException>(() => actor.GetLeftSeatCountAsync(1000, 10000));
        }

        [Test]
        public void UpdateCount_StationNotFound()
        {
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(TrainGran));
            using var mocker = claptrapDesign.CreateAutoMock("123", state);
            var actor = mocker.Create<TrainGran>();
            Assert.ThrowsAsync<StationNotFoundException>(() => actor.UpdateCountAsync(1000, 10000));
        }

        [Test]
        public async Task UpdateCount_Ok()
        {
            const int maxCount = 10000;
            var state = TrainInfo.Create(Enumerable.Range(1000, 4).ToList(), maxCount);
            var claptrapDesign = ActorTestHelper.GetDesign(typeof(TrainGran));
            using var mocker = claptrapDesign.CreateAutoMock("123", state);
            var actor = mocker.Create<TrainGran>();
            await actor.UpdateCountAsync(1000, 1001);
        }
    }
}