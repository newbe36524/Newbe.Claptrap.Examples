using Newbe.Claptrap;

namespace HelloClaptrap.Models.Sku.Events
{
    public class InventoryUpdateEvent : IEventData
    {
        public int Diff { get; set; }
        public int NewInventory { get; set; }
    }
}