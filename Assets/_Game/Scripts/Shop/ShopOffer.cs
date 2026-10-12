using System;

namespace PriceForgeClicker.Shop
{
    public sealed class ShopOffer
    {
        public ShopOffer(string sku, long price, long basePrice, DateTime? saleEndsAtUtc)
        {
            Sku = sku;
            Price = price;
            BasePrice = basePrice;
            SaleEndsAtUtc = saleEndsAtUtc;
        }

        public string Sku { get; }
        public long Price { get; }
        public long BasePrice { get; }
        public DateTime? SaleEndsAtUtc { get; }
        public bool IsOnSale => Price < BasePrice;
    }
}
