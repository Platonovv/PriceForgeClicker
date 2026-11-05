using System.Threading;
using Cysharp.Threading.Tasks;

namespace PriceForgeClicker.Network
{
    public sealed class AuthApi
    {
        private const string RegisterPath = "/api/v1/auth/register";
        private const string LoginPath = "/api/v1/auth/login";

        private readonly ApiClient _apiClient;
        private readonly AuthSession _session;

        public AuthApi(ApiClient apiClient, AuthSession session)
        {
            _apiClient = apiClient;
            _session = session;
        }

        public async UniTask RegisterAsync(string email, string password, CancellationToken cancellationToken)
        {
            await _apiClient.PostAsync(RegisterPath, new AuthRequest(email, password), cancellationToken);
        }

        public async UniTask LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            LoginClientResponse response = await _apiClient.PostAsync<LoginClientResponse>(
                LoginPath,
                new AuthRequest(email, password),
                cancellationToken);

            _session.SetAccessToken(response.AccessToken);
        }
    }
}
