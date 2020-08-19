using Autofac.Extras.Moq;
using Moq;
using Newbe.Claptrap;
using Newbe.Claptrap.Orleans;
using Orleans;

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

        public static void MockStateData<TStateData>(this AutoMock mocker, TStateData stateData)
            where TStateData : IStateData
        {
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Data)
                .Returns(stateData);
        }

        public static void MockStateIdentity(this AutoMock mocker, IClaptrapIdentity identity)
        {
            mocker.Mock<IClaptrapGrainCommonService>()
                .Setup(x => x.ClaptrapAccessor.Claptrap.State.Identity)
                .Returns(identity);
        }

        public static void MockGrain<TGrain>(this AutoMock mocker, string id, TGrain grain)
            where TGrain : IGrainWithStringKey
        {
            mocker.Mock<IGrainFactory>()
                .Setup(x => x.GetGrain<TGrain>(id, It.IsAny<string>()))
                .Returns<string, string>((primaryKey, grainClassNamePrefix) => grain);
        }
    }
}