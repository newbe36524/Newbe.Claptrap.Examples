using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using HelloClaptrap.Actors.Order;
using HelloClaptrap.IActor;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Order;
using HelloClaptrap.Models.Order.Events;
using Moq;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;
using NUnit.Framework;

namespace HelloClaptrap.Actors.Tests.Order
{
    public class OrderGrainTest
    {
        [Test]
        public async Task Success()
        {
            using var mocker = AutoMock.GetStrict();
            const string userId = "userId";
            var stateData = new OrderState
            {
                OrderCreated = false
            };
            var identity = new ClaptrapIdentity("666", ClaptrapCodes.OrderGrain);
            var createOrderInput = new CreateOrderInput
            {
                CartId = identity.Id,
                UserId = userId
            };
            const string skuId = "yueluo-666";
            const int skuCountInCart = 666;
            var cartItems = new Dictionary<string, int>
            {
                {skuId, skuCountInCart}
            };

            mocker.MockStateData(stateData);
            mocker.MockStateIdentity(identity);
            var cartGrain = new Mock<ICartGrain>();
            cartGrain.Setup(x => x.GetItemsAsync())
                .ReturnsAsync(cartItems);
            cartGrain.Setup(x => x.RemoveAllItemsAsync())
                .Returns(Task.CompletedTask);
            mocker.MockGrain(createOrderInput.CartId, cartGrain.Object);

            var skuGrain = new Mock<ISkuGrain>();
            skuGrain.Setup(x => x.UpdateInventoryAsync(skuCountInCart))
                .ReturnsAsync(0);
            mocker.MockGrain(skuId, skuGrain.Object);

            mocker.MockerEventCode<OrderCreatedEvent>(ClaptrapCodes.OrderCreated);

            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.HandleEventAsync(It.IsAny<IEvent>()))
                .Returns(Task.CompletedTask);
            var handler = mocker.Create<OrderGrain>();

            await handler.CreateOrderAsync(createOrderInput);
        }

        [Test]
        public Task AlreadyCreated()
        {
            using var mocker = AutoMock.GetStrict();
            var stateData = new OrderState
            {
                OrderCreated = true
            };
            mocker.MockStateData(stateData);

            var identity = new ClaptrapIdentity("666", ClaptrapCodes.OrderGrain);
            mocker.MockStateIdentity(identity);

            var handler = mocker.Create<OrderGrain>();
            var createOrderInput = new CreateOrderInput { };
            Assert.ThrowsAsync<BizException>(() => handler.CreateOrderAsync(createOrderInput));
            return Task.CompletedTask;
        }
    }
}