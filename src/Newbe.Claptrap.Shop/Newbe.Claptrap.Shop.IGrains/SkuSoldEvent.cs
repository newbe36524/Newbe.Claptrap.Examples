using System;

namespace Newbe.Claptrap.IGrain
{
    public class SkuSoldEvent : IEventData
    {
        public string BuyerUserId { get; set; }
        public DateTime SoldTime { get; set; }
    }
}