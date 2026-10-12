using PriceForgeClicker.Settings;
using PriceForgeClicker.Shop;

namespace PriceForgeClicker.Services
{
    public sealed class ClickerService
    {
        private readonly Wallet _wallet;
        private readonly ClickerState _state;

        public ClickerService(Wallet wallet, ClickerState state)
        {
            _wallet = wallet;
            _state = state;
        }

        public void Click() => _wallet.Add(_state.RewardPerClick);

        public void ApplyUpgrade(ShopItemDefinition item)
        {
            switch (item.Effect)
            {
                case UpgradeEffect.ClickPower:
                    _state.AddClickPower(item.EffectValue);
                    break;
                case UpgradeEffect.AutoIncome:
                    _state.AddIncome(item.EffectValue);
                    break;
                case UpgradeEffect.ClickMultiplier:
                    _state.AddMultiplier(item.EffectValue);
                    break;
            }
        }
    }
}
