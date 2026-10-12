using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Services;
using PriceForgeClicker.Settings;

namespace PriceForgeClicker.Shop
{
    public sealed class LocalShopService : IShopService
    {
        private readonly ShopSettings _shopSettings;
        private readonly Wallet _wallet;
        private readonly DateTime _saleEndsAtUtc;

        public LocalShopService(AssetProvider assetProvider, Wallet wallet)
        {
            _shopSettings = assetProvider.GetShopSettings();
            _wallet = wallet;
            _saleEndsAtUtc = DateTime.UtcNow.AddMinutes(_shopSettings.LocalSaleMinutes);
        }

        public UniTask<IReadOnlyList<ShopOffer>> GetOffersAsync(CancellationToken cancellationToken)
        {
            var offers = new List<ShopOffer>(_shopSettings.Items.Count);

            foreach (ShopItemDefinition item in _shopSettings.Items)
            {
                offers.Add(CreateOffer(item));
            }

            return UniTask.FromResult<IReadOnlyList<ShopOffer>>(offers);
        }

        public UniTask<PurchaseResult> PurchaseAsync(ShopOffer offer, CancellationToken cancellationToken)
        {
            PurchaseResult result = _wallet.TrySpend(offer.Price) ? PurchaseResult.Success : PurchaseResult.NotEnoughCoins;

            return UniTask.FromResult(result);
        }

        private ShopOffer CreateOffer(ShopItemDefinition item)
        {
            if (item.LocalSalePercent <= 0)
            {
                return new ShopOffer(item.Sku, item.LocalPrice, item.LocalPrice, null);
            }

            long salePrice = item.LocalPrice * (100 - item.LocalSalePercent) / 100;

            return new ShopOffer(item.Sku, salePrice, item.LocalPrice, _saleEndsAtUtc);
        }
    }
}
