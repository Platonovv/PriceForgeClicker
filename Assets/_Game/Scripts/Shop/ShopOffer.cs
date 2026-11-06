using System;

namespace PriceForgeClicker.Shop
{
    public sealed class ShopOffer
    {
        public ShopOffer(string productId, string title, long price, long basePrice, DateTime? saleEndsAtUtc)
        {
            ProductId = productId;
            Title = title;
            Price = price;
            BasePrice = basePrice;
            SaleEndsAtUtc = saleEndsAtUtc;
        }

        public string ProductId { get; }
        public string Title { get; }
        public long Price { get; }
        public long BasePrice { get; }
        public DateTime? SaleEndsAtUtc { get; }
        public bool IsOnSale => Price < BasePrice;
    }
}
