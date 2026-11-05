namespace PriceForgeClicker.Network
{
    public sealed record LoginClientResponse(string AccessToken)
    {
        public string AccessToken { get; } = AccessToken;
    }
}