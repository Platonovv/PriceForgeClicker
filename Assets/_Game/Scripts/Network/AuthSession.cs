namespace PriceForgeClicker.Network
{
    public sealed class AuthSession
    {
        public string AccessToken { get; private set; }

        public bool IsAuthorized => !string.IsNullOrEmpty(AccessToken);

        public void SetAccessToken(string accessToken) => AccessToken = accessToken;
    }
}
