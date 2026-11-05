using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace PriceForgeClicker.Network
{
    public sealed class ApiClient
    {
        private const string Address = "http://localhost:8080";
        private const string JsonContentType = "application/json";
        private const string AuthorizationHeader = "Authorization";
        private const string IdempotencyKeyHeader = "Idempotency-Key";

        private readonly AuthSession _session;

        public ApiClient(AuthSession session)
        {
            _session = session;
        }

        public async UniTask<T> GetAsync<T>(string path, CancellationToken cancellationToken)
        {
            string response = await SendAsync(UnityWebRequest.Get(Address + path), cancellationToken);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async UniTask PostAsync(string path, object body, CancellationToken cancellationToken)
        {
            await SendAsync(CreatePost(path, body, null), cancellationToken);
        }

        public async UniTask<T> PostAsync<T>(
            string path,
            object body,
            CancellationToken cancellationToken,
            string idempotencyKey = null)
        {
            string response = await SendAsync(CreatePost(path, body, idempotencyKey), cancellationToken);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private UnityWebRequest CreatePost(string path, object body, string idempotencyKey)
        {
            UnityWebRequest request
                = UnityWebRequest.Post(Address + path, JsonConvert.SerializeObject(body), JsonContentType);

            if (idempotencyKey != null)
            {
                request.SetRequestHeader(IdempotencyKeyHeader, idempotencyKey);
            }

            return request;
        }

        private async UniTask<string> SendAsync(UnityWebRequest request, CancellationToken cancellationToken)
        {
            using (request)
            {
                AddAuthorization(request);
                UnityWebRequest response = await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
                return response.downloadHandler.text;
            }
        }

        private void AddAuthorization(UnityWebRequest request)
        {
            if (_session.IsAuthorized)
            {
                request.SetRequestHeader(AuthorizationHeader, $"Bearer {_session.AccessToken}");
            }
        }
    }
}
