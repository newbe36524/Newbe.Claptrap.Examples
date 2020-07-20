using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.Ticketing.Actors.Tests
{
    public static class AutoMockExtensions
    {
        public static void MockEventHandling<TEventDataType>(this AutoMock mocker,
            string eventDataTypeCode)
        {
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapTypeCodeFactory.FindEventTypeCode(
                    It.IsAny<IClaptrapBox>(),
                    It.IsAny<TEventDataType>()))
                .Returns(eventDataTypeCode);
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.HandleEventAsync(It.IsAny<IEvent>()))
                .Returns(Task.CompletedTask);
        }
    }
}