using Newbe.Claptrap.Shop.Models;

namespace Newbe.Claptrap.IGrain.Domain.Sku.Master
{
    public class SkuStateData : IStateData
    {
        public string Id { get; set; }
        public SkuStatus Status { get; set; }
    }
}