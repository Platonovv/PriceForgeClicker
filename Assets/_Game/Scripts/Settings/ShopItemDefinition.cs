using System;
using PriceForgeClicker.Shop;
using UnityEngine;

namespace PriceForgeClicker.Settings
{
    [Serializable]
    public sealed class ShopItemDefinition
    {
        [SerializeField] private string _productId;
        [SerializeField] private Sprite _icon;
        [SerializeField] private UpgradeEffect _effect;
        [SerializeField] private long _effectValue;

        public string ProductId => _productId;
        public Sprite Icon => _icon;
        public UpgradeEffect Effect => _effect;
        public long EffectValue => _effectValue;
    }
}
