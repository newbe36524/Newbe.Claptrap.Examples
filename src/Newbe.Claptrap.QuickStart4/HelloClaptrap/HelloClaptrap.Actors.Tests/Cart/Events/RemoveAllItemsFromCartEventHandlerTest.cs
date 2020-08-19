using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using HelloClaptrap.Actors.Cart.Events;
using HelloClaptrap.Models.Cart;
using HelloClaptrap.Models.Cart.Events;
using NUnit.Framework;

namespace HelloClaptrap.Actors.Tests.Cart.Events
{
    public class RemoveAllItemsFromCartEventHandlerTest
    {
        [Test]
        public async Task RemoveSuccess()
        {
            using var mocker = AutoMock.GetStrict();

            await using var handler = mocker.Create<RemoveAllItemsFromCartEventHandler>();
            const string oldKey = "oneKey";
            var state = new CartState
            {
                Items = new Dictionary<string, int>
                {
                    {oldKey, 100}
                }
            };
            var evt = new RemoveAllItemsFromCartEvent();
            await handler.HandleEvent(state, evt, default);

            state.Items.Should().BeNull();
        }
    }
}