using PriceForgeClicker.UI;
using VContainer.Unity;

namespace PriceForgeClicker.Bootstrap
{
    public sealed class GameFlow : IStartable
    {
        private readonly ClickerMediator _clickerMediator;
        private readonly ShopMediator _shopMediator;

        public GameFlow(ClickerMediator clickerMediator, ShopMediator shopMediator)
        {
            _clickerMediator = clickerMediator;
            _shopMediator = shopMediator;
        }

        public void Start()
        {
            _clickerMediator.Show();
            _shopMediator.Show();
        }
    }
}
