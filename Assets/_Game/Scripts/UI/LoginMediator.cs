using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Network;

namespace PriceForgeClicker.UI
{
    public sealed class LoginMediator
    {
        private readonly LoginView _view;
        private readonly ApiClient _apiClient;

        public LoginMediator(LoginView view, ApiClient apiClient)
        {
            _view = view;
            _apiClient = apiClient;
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
            await _apiClient.RegisterAsync(_view.Email, _view.Password, ct);
            await LoginAsync(ct);
        }

        private async UniTask LoginAsync(CancellationToken ct)
            => await _apiClient.LoginAsync(_view.Email, _view.Password, ct);

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