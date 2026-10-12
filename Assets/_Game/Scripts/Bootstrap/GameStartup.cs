using PriceForgeClicker.Services;
using PriceForgeClicker.Settings;
using PriceForgeClicker.Shop;
using PriceForgeClicker.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PriceForgeClicker.Bootstrap
{
    public sealed class GameStartup : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] private GameSettings _gameSettings;

        [Header("Views")]
        [SerializeField] private ClickerView _clickerView;
        [SerializeField] private ShopView _shopView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterSettings(builder);
            RegisterServices(builder);
            RegisterUi(builder);
            builder.RegisterEntryPoint<GameFlow>();
        }

        private void RegisterSettings(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameSettings);
            builder.Register<AssetProvider>(Lifetime.Singleton);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<Wallet>(Lifetime.Singleton);
            builder.Register<ClickerState>(Lifetime.Singleton);
            builder.Register<ClickerService>(Lifetime.Singleton);
            builder.Register<IShopService, LocalShopService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<AutoIncomeTicker>();
        }

        private void RegisterUi(IContainerBuilder builder)
        {
            builder.RegisterComponent(_clickerView);
            builder.RegisterComponent(_shopView);
            builder.Register<ClickerMediator>(Lifetime.Singleton);
            builder.RegisterEntryPoint<ShopMediator>().AsSelf();
        }
    }
}
