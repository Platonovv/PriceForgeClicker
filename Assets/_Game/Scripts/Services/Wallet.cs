using System;

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

        public long PendingCoins { get; private set; }

        public long ConfirmedCoins { get; private set; }

        public void Subscribe(Action listener) => OnChanged += listener;

        public void Unsubscribe(Action listener) => OnChanged -= listener;

        public void Add(long amount)
        {
            Coins += amount;
            PendingCoins += amount;
        }

        public long TakePending(long max)
        {
            long pendingCoins = Math.Min(PendingCoins, max);
            PendingCoins -= pendingCoins;
            return pendingCoins;
        }

        public void SetBalance(long serverBalance)
        {
            ConfirmedCoins = serverBalance;
            Coins = serverBalance + PendingCoins;
        }

        public void SpendConfirmed(long amount)
        {
            ConfirmedCoins -= amount;
            Coins -= amount;
        }
    }
}
