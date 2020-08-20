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
        private const string SkuEventSuffix = "_e_" + SkuGrain;
        public const string SkuInventoryUpdate = "inventoryUpdate" + SkuEventSuffix;

        #endregion

        #region Order

        public const string OrderGrain = "order_claptrap_newbe";
        private const string OrderEventSuffix = "_e_" + OrderGrain;
        public const string OrderCreated = "orderCreated" + OrderEventSuffix;

        public const string OrderDbGrain = "db_order_claptrap_newbe";

        #endregion
    }
}