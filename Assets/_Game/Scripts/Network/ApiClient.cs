using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace PriceForgeClicker.Network
{
    public sealed class ApiClient
    {
        private const string Address = "http://localhost:8080";
        private const string RegisterPath = "/api/v1/auth/register";
        private const string LoginPath = "/api/v1/auth/login";

        public async UniTask RegisterAsync(string email, string password, CancellationToken cancellationToken)
        {
            AuthRequest authRequest = new AuthRequest(email, password);
            await PostJsonAsync(RegisterPath, authRequest, cancellationToken);
        }

        public async UniTask<string> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            AuthRequest authRequest = new AuthRequest(email, password);
            string response = await PostJsonAsync(LoginPath, authRequest, cancellationToken);
            return JsonConvert.DeserializeObject<LoginClientResponse>(response).AccessToken;
        }

        private async UniTask<string> PostJsonAsync(string path, object body, CancellationToken ct)
        {
            using UnityWebRequest request = UnityWebRequest.Post(Address + path, SerializeObject(body), "application/json");

            UnityWebRequest response = await request.SendWebRequest().ToUniTask(cancellationToken: ct);

            return response.downloadHandler.text;
        }

        private string SerializeObject(object body)
            => JsonConvert.SerializeObject(body);
    }
}