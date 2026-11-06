using System.Collections.Generic;
using UnityEngine;

namespace PriceForgeClicker.Settings
{
    [CreateAssetMenu(menuName = "SO/Shop Settings")]
    public sealed class ShopSettings : ScriptableObject
    {
        [Header("Items")]
        [SerializeField] private List<ShopItemDefinition> _items = new();

        [Header("Local Sale")]
        [SerializeField] private int _localSaleMinutes = 30;

        public IReadOnlyList<ShopItemDefinition> Items => _items;
        public int LocalSaleMinutes => _localSaleMinutes;

        public ShopItemDefinition FindByProductId(string productId)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].ProductId == productId)
                {
                    return _items[i];
                }
            }

            return null;
        }
    }
}
