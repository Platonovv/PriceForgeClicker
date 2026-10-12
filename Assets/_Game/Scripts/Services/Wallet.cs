using System;
using PriceForgeClicker.Settings;

namespace PriceForgeClicker.Services
{
    public sealed class Wallet : IReactiveObject
    {
        private long _coins;

        public Wallet(AssetProvider assetProvider)
        {
            _coins = assetProvider.GetClickerSettings().StartCoins;
        }

        public event Action OnChanged;

        public long Coins
        {
            get => _coins;
            private set
            {
                _coins = value;
                OnChanged?.Invoke();
            }
        }

        public void Subscribe(Action listener) => OnChanged += listener;

        public void Unsubscribe(Action listener) => OnChanged -= listener;

        public void Add(long amount) => Coins += amount;

        public bool TrySpend(long amount)
        {
            if (_coins < amount)
            {
                return false;
            }

            Coins -= amount;
            return true;
        }
    }
}
