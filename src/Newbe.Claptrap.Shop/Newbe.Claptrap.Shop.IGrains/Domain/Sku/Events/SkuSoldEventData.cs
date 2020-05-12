using System;

namespace Newbe.Claptrap.IGrain.Domain.Sku.Events
{
    public class SkuSoldEventData : IEventData
    {
        public string BuyerUserId { get; set; }
        public DateTime SoldTime { get; set; }
    }
}