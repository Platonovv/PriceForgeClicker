using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Services;
using PriceForgeClicker.Settings;
using PriceForgeClicker.Shop;
using UnityEngine;
using VContainer.Unity;

namespace PriceForgeClicker.UI
{
    public sealed class ShopMediator : ITickable
    {
        private const float RefreshSeconds = 1f;

        private readonly ShopView _view;
        private readonly IShopService _shopService;
        private readonly Wallet _wallet;
        private readonly ClickerService _clickerService;
        private readonly AssetProvider _assetProvider;
        private readonly List<ShopItemElement> _elements = new();
        private readonly CancellationTokenSource _lifetime = new();
        private float _sinceRefresh;
        private bool _isPurchasing;

        public ShopMediator(ShopView view,
                            IShopService shopService,
                            Wallet wallet,
                            ClickerService clickerService,
                            AssetProvider assetProvider)
        {
            _view = view;
            _shopService = shopService;
            _wallet = wallet;
            _clickerService = clickerService;
            _assetProvider = assetProvider;
        }

        public void Show()
        {
            _wallet.Subscribe(UpdateElements);
            LoadOffersAsync(_lifetime.Token).Forget();
        }

        public void Tick()
        {
            _sinceRefresh += Time.deltaTime;

            if (_sinceRefresh < RefreshSeconds)
            {
                return;
            }

            _sinceRefresh = 0;
            UpdateElements();
        }

        private async UniTaskVoid LoadOffersAsync(CancellationToken cancellationToken)
        {
            _view.ShowStatus("Загрузка магазина...");
            IReadOnlyList<ShopOffer> offers = await _shopService.GetOffersAsync(cancellationToken);
            CreateElements(offers);
            _view.HideStatus();
        }

        private void CreateElements(IReadOnlyList<ShopOffer> offers)
        {
            ShopSettings shopSettings = _assetProvider.GetShopSettings();
            ShopItemElement prefab = _assetProvider.GetUiSettings().ShopItemElementPrefab;

            foreach (ShopOffer offer in offers)
            {
                ShopItemDefinition definition = shopSettings.FindBySku(offer.Sku);

                if (definition == null)
                {
                    continue;
                }

                ShopItemElement element = Object.Instantiate(prefab, _view.Content);
                element.Init(definition, offer, _wallet, RequestPurchase);
                _elements.Add(element);
            }
        }

        private void RequestPurchase(ShopItemElement element) => PurchaseAsync(element, _lifetime.Token).Forget();

        private async UniTaskVoid PurchaseAsync(ShopItemElement element, CancellationToken cancellationToken)
        {
            if (_isPurchasing)
            {
                return;
            }

            _isPurchasing = true;
            PurchaseResult result = await _shopService.PurchaseAsync(element.Offer, cancellationToken);
            _isPurchasing = false;

            if (result == PurchaseResult.Success)
            {
                _clickerService.ApplyUpgrade(element.Definition);
            }
        }

        private void UpdateElements()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].UpdateView();
            }
        }
    }
}
