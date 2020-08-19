using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using HelloClaptrap.Actors.Sku;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Models.Sku.Events;
using Moq;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;
using NUnit.Framework;

namespace HelloClaptrap.Actors.Tests.Sku
{
    public class SkuGrainTest
    {
        [Test]
        public async Task GetItemsAsync()
        {
            using var mocker = AutoMock.GetStrict();

            var stateData = new SkuState
            {
                Inventory = 666
            };
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(stateData);

            var handler = mocker.Create<SkuGrain>();
            var items = await handler.GetInventoryAsync();
            items.Should().Be(stateData.Inventory);
        }

        [Test]
        public async Task UpdateInventorySuccess()
        {
            using var mocker = AutoMock.GetStrict();
            const int sourceInventory = 666;
            const int diff = -66;
            var stateData = new SkuState
            {
                Inventory = sourceInventory
            };
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(stateData);

            mocker.MockerEventCode<InventoryUpdateEvent>(ClaptrapCodes.SkuInventoryUpdate);

            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.HandleEventAsync(It.IsAny<IEvent>()))
                .Callback<IEvent>(evt => { stateData.Inventory += diff; })
                .Returns<IEvent>(evt => Task.CompletedTask);

            var handler = mocker.Create<SkuGrain>();
            var inventory = await handler.UpdateInventoryAsync(diff);
            inventory.Should().Be(sourceInventory + diff);
        }

        [Test]
        public Task UpdateInventoryFailed()
        {
            using var mocker = AutoMock.GetStrict();
            const int sourceInventory = 666;
            const int diff = -667;
            var stateData = new SkuState
            {
                Inventory = sourceInventory
            };
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(stateData);

            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.HandleEventAsync(It.IsAny<IEvent>()))
                .Callback<IEvent>(evt => { stateData.Inventory += diff; })
                .Returns<IEvent>(evt => Task.CompletedTask);

            var handler = mocker.Create<SkuGrain>();
            Assert.ThrowsAsync<BizException>(() => handler.UpdateInventoryAsync(diff));
            return Task.CompletedTask;
        }

        [Test]
        public Task UpdateInventoryFailed_Diff0()
        {
            using var mocker = AutoMock.GetStrict();
            const int sourceInventory = 666;
            const int diff = 0;
            var stateData = new SkuState
            {
                Inventory = sourceInventory
            };
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(stateData);

            var handler = mocker.Create<SkuGrain>();
            Assert.ThrowsAsync<BizException>(() => handler.UpdateInventoryAsync(diff));
            return Task.CompletedTask;
        }
    }
}