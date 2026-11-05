using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Network;
using PriceForgeClicker.Services;

namespace PriceForgeClicker.UI
{
    public sealed class LoginMediator
    {
        private readonly Wallet _wallet;
        private readonly LoginView _view;
        private readonly AuthApi _authApi;
        private readonly WalletApi _walletApi;

        public LoginMediator(LoginView view, AuthApi authApi, WalletApi walletApi, Wallet wallet)
        {
            _view = view;
            _wallet = wallet;
            _authApi = authApi;
            _walletApi = walletApi;
        }

        public event Action OnLoggedIn;

        public void Show()
        {
            _view.OnRegisterClicked += Register;
            _view.OnLoginClicked += Login;
            _view.Show();
        }

        public void Hide()
        {
            _view.OnRegisterClicked -= Register;
            _view.OnLoginClicked -= Login;
            _view.Hide();
        }

        private void Register() => RunAsync(RegisterAndLoginAsync).Forget();

        private void Login() => RunAsync(LoginAsync).Forget();

        private async UniTaskVoid RunAsync(Func<CancellationToken, UniTask> request)
        {
            _view.SetBusy(true);
            _view.ShowError(string.Empty);
            try
            {
                await request(_view.destroyCancellationToken);
                OnLoggedIn?.Invoke();
            }
            catch (UnityWebRequestException e)
            {
                ShowError(e.ResponseCode);
            }
            finally
            {
                _view.SetBusy(false);
            }
        }

        private async UniTask RegisterAndLoginAsync(CancellationToken ct)
        {
            await _authApi.RegisterAsync(_view.Email, _view.Password, ct);
            await LoginAsync(ct);
        }

        private async UniTask LoginAsync(CancellationToken ct)
        {
            await _authApi.LoginAsync(_view.Email, _view.Password, ct);
            long balance = await _walletApi.GetBalanceAsync(ct);
            _wallet.SetBalance(balance);
        }

        private void ShowError(long responseCode)
        {
            switch (responseCode)
            {
                case 401:
                    ShowError("Неверная почта или пароль");
                    break;
                case 409:
                    ShowError("Такая почта уже есть");
                    break;
                case 400:
                    ShowError("Проверь почту и пароль");
                    break;
                case 429:
                    ShowError("Слишком много попыток, подожди");
                    break;
                case 0:
                    ShowError("Нет связи с сервером");
                    break;
                default:
                    ShowError("Ошибка сервера");
                    break;
            }
        }

        private void ShowError(string message) => _view.ShowError(message);
    }
}