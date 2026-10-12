using PriceForgeClicker.Settings;

namespace PriceForgeClicker.Services
{
    public sealed class AssetProvider
    {
        private readonly GameSettings _gameSettings;

        public AssetProvider(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public ClickerSettings GetClickerSettings() => _gameSettings.ClickerSettings;

        public ShopSettings GetShopSettings() => _gameSettings.ShopSettings;

        public UiSettings GetUiSettings() => _gameSettings.UiSettings;
    }
}
