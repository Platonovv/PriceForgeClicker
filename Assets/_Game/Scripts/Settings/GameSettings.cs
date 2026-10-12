using UnityEngine;

namespace PriceForgeClicker.Settings
{
    [CreateAssetMenu(menuName = "SO/Game Settings")]
    public sealed class GameSettings : ScriptableObject
    {
        [Header("Features")]
        [SerializeField] private ClickerSettings _clickerSettings;
        [SerializeField] private ShopSettings _shopSettings;
        [SerializeField] private UiSettings _uiSettings;

        public ClickerSettings ClickerSettings => _clickerSettings;
        public ShopSettings ShopSettings => _shopSettings;
        public UiSettings UiSettings => _uiSettings;
    }
}
