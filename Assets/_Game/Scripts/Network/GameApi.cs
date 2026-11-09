using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PriceForgeClicker.Network
{
    public sealed class GameApi
    {
        private const string ItemsPath = "/api/v1/game/items";
        private const string PurchasePath = "/api/v1/game/purchases";

        private readonly ApiClient _apiClient;

        public GameApi(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public UniTask<List<GameItemResponse>> GetItemsAsync(CancellationToken cancellationToken)
            => _apiClient.GetAsync<List<GameItemResponse>>(ItemsPath, cancellationToken);

        public UniTask<PurchaseItemResponse> PurchaseAsync(string productId,
                                                           string idempotencyKey,
                                                           CancellationToken cancellationToken)
        {
            var request = new PurchaseItemRequest(productId);
            return _apiClient.PostAsync<PurchaseItemResponse>(PurchasePath, request, cancellationToken, idempotencyKey);
        }
    }
}