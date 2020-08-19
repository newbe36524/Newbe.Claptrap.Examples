using Autofac.Extras.Moq;
using Moq;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;

namespace HelloClaptrap.Actors.Tests
{
    public static class MockerExtensions
    {
        public static void MockerEventCode<TEvent>(this AutoMock mocker, string eventCode)
            where TEvent : IEventData
        {
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapTypeCodeFactory.FindEventTypeCode(
                    It.IsAny<IClaptrapBox>(),
                    It.IsAny<TEvent>()))
                .Returns(eventCode);
        }
    }
}