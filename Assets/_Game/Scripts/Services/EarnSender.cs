using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Network;
using UnityEngine;

namespace PriceForgeClicker.Services
{
    public sealed class EarnSender : IDisposable
    {
        private const int EarnDelay = 3000;
        private const long MaxBatchCoins = 60;

        private readonly Wallet _wallet;
        private readonly WalletApi _walletApi;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private EarnBatch _currentBatch;

        public EarnSender(Wallet wallet, WalletApi walletApi)
        {
            _wallet = wallet;
            _walletApi = walletApi;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start() => SendLoopAsync(_cancellationTokenSource.Token).Forget();

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private async UniTaskVoid SendLoopAsync(CancellationToken ct)
        {
            while (true)
            {
                try
                {
                    await UniTask.Delay(EarnDelay, cancellationToken: ct);

                    if (_currentBatch == null)
                    {
                        long pendingCoins = _wallet.TakePending(MaxBatchCoins);

                        if (pendingCoins <= 0)
                            continue;

                        _currentBatch = new EarnBatch(pendingCoins, Guid.NewGuid().ToString());
                    }

                    long balance = await _walletApi.EarnAsync(_currentBatch.Coins, _currentBatch.Key, ct);

                    _currentBatch = null;
                    _wallet.SetBalance(balance);
                }
                catch (UnityWebRequestException e)
                {
                    if (e.ResponseCode == 422)
                    {
                        Debug.LogError(e.Text);
                        _currentBatch = null;
                    }
                    else
                    {
                        Debug.LogWarning($"{e.ResponseCode} {e.Text}");
                    }
                }
            }
        }
    }
}