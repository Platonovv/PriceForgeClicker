namespace PriceForgeClicker.Network
{
    public sealed record EarnRequest(long Coins)
    {
        public long Coins { get; } = Coins;
    }
}