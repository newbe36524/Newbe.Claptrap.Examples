using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using HelloClaptrap.Actors.Sku.Events;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Models.Sku.Events;
using NUnit.Framework;

namespace HelloClaptrap.Actors.Tests.Sku.Events
{
    public class InventoryUpdateEventHandlerTest
    {
        [Test]
        public async Task RemoveSuccess()
        {
            using var mocker = AutoMock.GetStrict();

            await using var handler = mocker.Create<InventoryUpdateEventHandler>();

            var stateData = new SkuState
            {
                Inventory = 789
            };
            var inventoryUpdateEvent = new InventoryUpdateEvent
            {
                NewInventory = 666,
                Diff = -66
            };
            await handler.HandleEvent(stateData, inventoryUpdateEvent, default);
            stateData.Inventory.Should().Be(inventoryUpdateEvent.NewInventory);
        }
    }
}