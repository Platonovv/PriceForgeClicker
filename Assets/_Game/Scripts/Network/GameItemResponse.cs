namespace PriceForgeClicker.Network
{
    public sealed record GameItemResponse(string ProductId, string Name, long CoinPrice)
    {
        public string ProductId { get; } = ProductId;
        public string Name { get; } = Name;
        public long CoinPrice { get; } = CoinPrice;
    }
}