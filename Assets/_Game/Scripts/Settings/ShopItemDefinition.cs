using System;
using PriceForgeClicker.Shop;
using UnityEngine;

namespace PriceForgeClicker.Settings
{
    [Serializable]
    public sealed class ShopItemDefinition
    {
        [SerializeField] private string _sku;
        [SerializeField] private string _title;
        [SerializeField] private Sprite _icon;
        [SerializeField] private UpgradeEffect _effect;
        [SerializeField] private long _effectValue;
        [SerializeField] private long _localPrice;
        [SerializeField] private int _localSalePercent;

        public string Sku => _sku;
        public string Title => _title;
        public Sprite Icon => _icon;
        public UpgradeEffect Effect => _effect;
        public long EffectValue => _effectValue;
        public long LocalPrice => _localPrice;
        public int LocalSalePercent => _localSalePercent;
    }
}
