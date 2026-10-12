using PriceForgeClicker.UI;
using UnityEngine;

namespace PriceForgeClicker.Settings
{
    [CreateAssetMenu(menuName = "SO/UI Settings")]
    public sealed class UiSettings : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private ShopItemElement _shopItemElementPrefab;
        [SerializeField] private FloatingRewardElement _floatingRewardPrefab;

        [Header("Click Feedback")]
        [SerializeField] private int _floatingRewardPoolSize = 12;
        [SerializeField] private float _floatingRewardSpread = 180f;
        [SerializeField] private float _punchScale = 0.9f;
        [SerializeField] private float _punchDuration = 0.12f;

        public ShopItemElement ShopItemElementPrefab => _shopItemElementPrefab;
        public FloatingRewardElement FloatingRewardPrefab => _floatingRewardPrefab;
        public int FloatingRewardPoolSize => _floatingRewardPoolSize;
        public float FloatingRewardSpread => _floatingRewardSpread;
        public float PunchScale => _punchScale;
        public float PunchDuration => _punchDuration;
    }
}
