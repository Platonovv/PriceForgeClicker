using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Network;
using PriceForgeClicker.Services;

namespace PriceForgeClicker.Shop
{
    public sealed class ServerShopService : IShopService
    {
        private readonly GameApi _gameApi;
        private readonly Wallet _wallet;

        public ServerShopService(GameApi gameApi, Wallet wallet)
        {
            _gameApi = gameApi;
            _wallet = wallet;
        }

        public async UniTask<IReadOnlyList<ShopOffer>> GetOffersAsync(CancellationToken cancellationToken)
        {
            List<GameItemResponse> itemsList = await _gameApi.GetItemsAsync(cancellationToken);

            return itemsList.Select(gameItemResponse
                                        => new ShopOffer(gameItemResponse.ProductId,
                                                         gameItemResponse.Name,
                                                         gameItemResponse.CoinPrice,
                                                         gameItemResponse.CoinPrice,
                                                         null))
                            .ToList();
        }

        public UniTask<PurchaseResult> PurchaseAsync(ShopOffer offer, CancellationToken cancellationToken)
        {
            PurchaseResult result = _wallet.TrySpend(offer.Price)
                                        ? PurchaseResult.Success
                                        : PurchaseResult.NotEnoughCoins;

            return UniTask.FromResult(result);
        }
    }
}