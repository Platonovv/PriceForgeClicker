namespace PriceForgeClicker.Network
{
    public sealed record EarnBatch(long Coins, string Key)
    {
        public long Coins { get; } = Coins;
        public string Key { get; } = Key;
    }
}