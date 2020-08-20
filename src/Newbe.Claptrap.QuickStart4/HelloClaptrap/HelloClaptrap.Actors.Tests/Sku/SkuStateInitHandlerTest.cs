using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using HelloClaptrap.Actors.Sku;
using HelloClaptrap.Models;
using HelloClaptrap.Models.Sku;
using HelloClaptrap.Repository;
using Moq;
using Newbe.Claptrap;
using NUnit.Framework;

namespace HelloClaptrap.Actors.Tests.Sku
{
    public class SkuStateInitHandlerTest
    {
        [Test]
        [TestCase("yueluo-666", 666)]
        [TestCase("robot-777", 10)]
        public async Task Success(string id, int inventory)
        {
            using var mocker = AutoMock.GetStrict();
            mocker.Mock<ISkuRepository>()
                .Setup(x => x.GetInitInventoryAsync(id))
                .ReturnsAsync(inventory);

            var handler = mocker.Create<SkuStateInitHandler>();
            var identity = new ClaptrapIdentity(id, ClaptrapCodes.SkuGrain);
            var data = (SkuState) await handler.Create(identity);
            data.Inventory.Should().Be(inventory);
        }

        [Test]
        public Task Failed()
        {
            using var mocker = AutoMock.GetStrict();
            const string skuId = "yueluo-666";
            mocker.Mock<ISkuRepository>()
                .Setup(x => x.GetInitInventoryAsync(skuId))
                .Throws<BizException>();

            var handler = mocker.Create<SkuStateInitHandler>();
            var identity = new ClaptrapIdentity(skuId, ClaptrapCodes.SkuGrain);
            Assert.ThrowsAsync<BizException>(() => handler.Create(identity));
            return Task.CompletedTask;
        }
    }
}