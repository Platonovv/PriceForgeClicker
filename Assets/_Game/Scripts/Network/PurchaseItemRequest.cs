namespace PriceForgeClicker.Network
{
    public sealed record PurchaseItemRequest(string ProductId)
    {
        public string ProductId { get; } = ProductId;
    }
}