using System;

namespace PriceForgeClicker.Network
{
    public sealed record PurchaseItemResponse(Guid PurchaseId, string ProductId, long CoinPrice, DateTime CreatedAt)
    {
        public Guid PurchaseId { get; } = PurchaseId;
        public string ProductId { get; } = ProductId;
        public long CoinPrice { get; } = CoinPrice;
        public DateTime CreatedAt { get; } = CreatedAt;
    }
}