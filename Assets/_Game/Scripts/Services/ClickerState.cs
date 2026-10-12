using System;

namespace PriceForgeClicker.Services
{
    public sealed class ClickerState : IReactiveObject
    {
        private long _clickReward;

        public ClickerState(AssetProvider assetProvider)
        {
            _clickReward = assetProvider.GetClickerSettings().BaseClickReward;
            ClickMultiplier = 1;
        }

        public event Action OnChanged;

        public long ClickReward
        {
            get => _clickReward;
            private set
            {
                _clickReward = value;
                OnChanged?.Invoke();
            }
        }

        public long IncomePerSecond { get; private set; }
        public long ClickMultiplier { get; private set; }
        public long RewardPerClick => _clickReward * ClickMultiplier;

        public void Subscribe(Action listener) => OnChanged += listener;

        public void Unsubscribe(Action listener) => OnChanged -= listener;

        public void AddClickPower(long value) => ClickReward = _clickReward + value;

        public void AddIncome(long value)
        {
            IncomePerSecond += value;
            ClickReward = _clickReward;
        }

        public void AddMultiplier(long value)
        {
            ClickMultiplier += value;
            ClickReward = _clickReward;
        }
    }
}
