using PriceForgeClicker.Services;
using PriceForgeClicker.UI;
using VContainer.Unity;

namespace PriceForgeClicker.Bootstrap
{
    public sealed class GameFlow : IStartable
    {
        private readonly LoginMediator _loginMediator;
        private readonly ClickerMediator _clickerMediator;
        private readonly ShopMediator _shopMediator;
        private readonly EarnSender _earnSender;

        public GameFlow(LoginMediator loginMediator,
                        ClickerMediator clickerMediator,
                        ShopMediator shopMediator,
                        EarnSender earnSender)
        {
            _earnSender = earnSender;
            _loginMediator = loginMediator;
            _clickerMediator = clickerMediator;
            _shopMediator = shopMediator;
        }

        public void Start()
        {
            _loginMediator.OnLoggedIn += StartGame;
            _loginMediator.Show();
        }

        private void StartGame()
        {
            _loginMediator.OnLoggedIn -= StartGame;
            _loginMediator.Hide();
            _clickerMediator.Show();
            _shopMediator.Show();
            _earnSender.Start();
        }
    }
}