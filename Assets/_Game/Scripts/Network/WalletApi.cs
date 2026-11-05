using System.Threading;
using Cysharp.Threading.Tasks;

namespace PriceForgeClicker.Network
{
    public sealed class WalletApi
    {
        private const string WalletPath = "/api/v1/wallet";
        private const string EarnPath = "/api/v1/wallet/earn";

        private readonly ApiClient _apiClient;

        public WalletApi(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public UniTask<long> GetBalanceAsync(CancellationToken cancellationToken)
            => _apiClient.GetAsync<long>(WalletPath, cancellationToken);

        public UniTask<long> EarnAsync(long coins, string idempotencyKey, CancellationToken cancellationToken)
            => _apiClient.PostAsync<long>(EarnPath, new EarnRequest(coins), cancellationToken, idempotencyKey);
    }
}
