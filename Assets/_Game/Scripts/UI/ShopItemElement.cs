using System;
using PriceForgeClicker.Services;
using PriceForgeClicker.Settings;
using PriceForgeClicker.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PriceForgeClicker.UI
{
    public sealed class ShopItemElement : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _effectText;

        [Header("Price")]
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _oldPriceText;
        [SerializeField] private GameObject _saleBadge;
        [SerializeField] private TMP_Text _saleBadgeText;
        [SerializeField] private GameObject _saleTimer;
        [SerializeField] private TMP_Text _saleTimerText;

        [Header("Buy")]
        [SerializeField] private Button _buyButton;

        private ShopItemDefinition _definition;
        private ShopOffer _offer;
        private Wallet _wallet;
        private Action<ShopItemElement> _buyRequested;

        public ShopItemDefinition Definition => _definition;
        public ShopOffer Offer => _offer;

        public void Init(ShopItemDefinition definition, ShopOffer offer, Wallet wallet, Action<ShopItemElement> buyRequested)
        {
            _definition = definition;
            _offer = offer;
            _wallet = wallet;
            _buyRequested = buyRequested;
            _buyButton.onClick.AddListener(Buy);
            ShowStaticInfo();
            UpdateView();
        }

        public void UpdateView()
        {
            _buyButton.interactable = _wallet.Coins >= _offer.Price;
            UpdateSaleTimer();
        }

        private void ShowStaticInfo()
        {
            _icon.sprite = _definition.Icon;
            _titleText.SetText(_definition.Title);
            _effectText.SetText(DescribeEffect());
            _priceText.SetText($"{_offer.Price}");
            _oldPriceText.gameObject.SetActive(_offer.IsOnSale);
            _oldPriceText.SetText($"{_offer.BasePrice}");
            _saleBadge.SetActive(_offer.IsOnSale);
            _saleTimer.SetActive(_offer.IsOnSale && _offer.SaleEndsAtUtc.HasValue);
            _saleBadgeText.SetText($"-{DiscountPercent()}%");
        }

        private long DiscountPercent()
        {
            if (_offer.BasePrice == 0)
            {
                return 0;
            }

            return (_offer.BasePrice - _offer.Price) * 100 / _offer.BasePrice;
        }

        private void UpdateSaleTimer()
        {
            if (!_offer.IsOnSale || !_offer.SaleEndsAtUtc.HasValue)
            {
                return;
            }

            TimeSpan left = _offer.SaleEndsAtUtc.Value - DateTime.UtcNow;

            if (left < TimeSpan.Zero)
            {
                left = TimeSpan.Zero;
            }

            _saleTimerText.SetText($"{(int)left.TotalMinutes:00}:{left.Seconds:00}");
        }

        private string DescribeEffect()
        {
            return _definition.Effect switch
            {
                UpgradeEffect.ClickPower => $"+{_definition.EffectValue} за клик",
                UpgradeEffect.AutoIncome => $"+{_definition.EffectValue} в секунду",
                UpgradeEffect.ClickMultiplier => $"x{_definition.EffectValue + 1} к клику",
                _ => string.Empty,
            };
        }

        private void Buy() => _buyRequested(this);
    }
}
