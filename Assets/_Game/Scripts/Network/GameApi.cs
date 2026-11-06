using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PriceForgeClicker.Network
{
    public sealed class GameApi
    {
        private const string ItemsPath = "/api/v1/game/items";

        private readonly ApiClient _apiClient;

        public GameApi(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public UniTask<List<GameItemResponse>> GetItemsAsync(CancellationToken cancellationToken)
            => _apiClient.GetAsync<List<GameItemResponse>>(ItemsPath, cancellationToken);
    }
}