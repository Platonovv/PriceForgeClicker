using UnityEngine;
using VContainer.Unity;

namespace PriceForgeClicker.Services
{
    public sealed class AutoIncomeTicker : ITickable
    {
        private const float TickSeconds = 1f;

        private readonly Wallet _wallet;
        private readonly ClickerState _state;
        private float _elapsed;

        public AutoIncomeTicker(Wallet wallet, ClickerState state)
        {
            _wallet = wallet;
            _state = state;
        }

        public void Tick()
        {
            if (_state.IncomePerSecond == 0)
            {
                return;
            }

            _elapsed += Time.deltaTime;

            if (_elapsed < TickSeconds)
            {
                return;
            }

            _elapsed -= TickSeconds;
            _wallet.Add(_state.IncomePerSecond);
        }
    }
}
