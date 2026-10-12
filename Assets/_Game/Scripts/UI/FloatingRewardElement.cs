using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PriceForgeClicker.UI
{
    public sealed class FloatingRewardElement : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;

        [Header("Motion")]
        [SerializeField] private float _riseDistance = 220f;
        [SerializeField] private float _duration = 0.7f;

        private int _playVersion;

        public void Play(long reward, Vector2 startPosition, CancellationToken cancellationToken)
        {
            _playVersion++;
            _text.SetText($"+{reward}");
            gameObject.SetActive(true);
            AnimateAsync(startPosition, _playVersion, cancellationToken).Forget();
        }

        private async UniTaskVoid AnimateAsync(Vector2 startPosition, int version, CancellationToken cancellationToken)
        {
            float elapsed = 0f;

            while (elapsed < _duration && version == _playVersion)
            {
                float progress = elapsed / _duration;
                _rectTransform.anchoredPosition = startPosition + Vector2.up * (_riseDistance * progress);
                _text.alpha = 1f - progress;
                elapsed += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            if (version == _playVersion)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
