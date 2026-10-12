using PriceForgeClicker.Services;

namespace PriceForgeClicker.UI
{
    public sealed class ClickerMediator
    {
        private readonly ClickerView _view;
        private readonly Wallet _wallet;
        private readonly ClickerState _state;
        private readonly ClickerService _clickerService;
        private readonly AssetProvider _assetProvider;

        public ClickerMediator(ClickerView view,
                               Wallet wallet,
                               ClickerState state,
                               ClickerService clickerService,
                               AssetProvider assetProvider)
        {
            _view = view;
            _wallet = wallet;
            _state = state;
            _clickerService = clickerService;
            _assetProvider = assetProvider;
        }

        public void Show()
        {
            _view.Init(_wallet, _state, _clickerService, _assetProvider);
            _view.Show();
        }
    }
}
