using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using HelloClaptrap.Actors.Order.Events;
using HelloClaptrap.Models.Order;
using HelloClaptrap.Models.Order.Events;
using NUnit.Framework;

namespace HelloClaptrap.Actors.Tests.Order.Events
{
    public class OrderCreatedEventHandlerTest
    {
        [Test]
        public async Task AlreadyCreated()
        {
            using var mocker = AutoMock.GetStrict();

            await using var handler = mocker.Create<OrderCreatedEventHandler>();

            var stateData = new OrderState
            {
                OrderCreated = false
            };
            var orderCreatedEvent = new OrderCreatedEvent
            {
                UserId = "666",
                Skus = new Dictionary<string, int>
                {
                    {"yueluo-666", 666}
                }
            };
            await handler.HandleEvent(stateData, orderCreatedEvent, default);
            stateData.OrderCreated.Should().BeTrue();
        }
    }
}