using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Network;
using PriceForgeClicker.Services;
using UnityEngine;

namespace PriceForgeClicker.Shop
{
    public sealed class ServerShopService : IShopService
    {
        private readonly GameApi _gameApi;
        private readonly Wallet _wallet;

        private string _idempotencyKey;
        private string _keyProductId;

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

        public async UniTask<PurchaseResult> PurchaseAsync(ShopOffer offer, CancellationToken cancellationToken)
        {
            try
            {
                if (_keyProductId != offer.ProductId)
                {
                    _keyProductId = offer.ProductId;
                    _idempotencyKey = Guid.NewGuid().ToString();
                }

                PurchaseItemResponse purchase = await _gameApi.PurchaseAsync(offer.ProductId, _idempotencyKey, cancellationToken);
                _wallet.SpendConfirmed(purchase.CoinPrice);
                return ResetKey(PurchaseResult.Success);
            }
            catch (UnityWebRequestException e)
            {
                switch (e.ResponseCode)
                {
                    case 0:
                    case >= 500:
                        Debug.LogWarning($"{e.ResponseCode} {e.Text}");
                        return PurchaseResult.Failed;
                    case 400:
                        return ResetKey(PurchaseResult.NotEnoughCoins);
                    default:
                        Debug.LogWarning($"{e.ResponseCode} {e.Text}");
                        return ResetKey(PurchaseResult.Failed);
                }
            }
        }

        private PurchaseResult ResetKey(PurchaseResult result)
        {
            _idempotencyKey = null;
            _keyProductId = null;
            return result;
        }
    }
}