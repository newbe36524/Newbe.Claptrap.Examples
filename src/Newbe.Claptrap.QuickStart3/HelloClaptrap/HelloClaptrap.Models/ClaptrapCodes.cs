namespace HelloClaptrap.Models
{
    public static class ClaptrapCodes
    {
        #region Cart

        public const string CartGrain = "cart_claptrap_newbe";
        private const string CartEventSuffix = "_e_" + CartGrain;
        public const string AddItemToCart = "addItem" + CartEventSuffix;
        public const string RemoveItemFromCart = "removeItem" + CartEventSuffix;
        public const string RemoveAllItemsFromCart = "remoeAllItems" + CartEventSuffix;

        #endregion

        #region Sku

        public const string SkuGrain = "sku_claptrap_newbe";
        private const string SkuEventSuffix = "_e_" + CartGrain;
        public const string SkuInventoryUpdate = "inventoryUpdate" + SkuEventSuffix;

        #endregion
    }
}