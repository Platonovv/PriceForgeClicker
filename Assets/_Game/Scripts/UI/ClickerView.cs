using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PriceForgeClicker.Services;
using PriceForgeClicker.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PriceForgeClicker.UI
{
    public sealed class ClickerView : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Button _clickButton;
        [SerializeField] private Image _clickTargetImage;
        [SerializeField] private RectTransform _clickTargetTransform;
        [SerializeField] private Image _coinIcon;
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _clickRewardText;
        [SerializeField] private TMP_Text _incomeText;

        [Header("Feedback")]
        [SerializeField] private RectTransform _floatingRewardContainer;

        private readonly List<FloatingRewardElement> _floatingRewards = new();
        private Wallet _wallet;
        private ClickerState _state;
        private ClickerService _clickerService;
        private UiSettings _uiSettings;
        private int _nextFloatingReward;
        private int _punchVersion;

        public void Init(Wallet wallet, ClickerState state, ClickerService clickerService, AssetProvider assetProvider)
        {
            _wallet = wallet;
            _state = state;
            _clickerService = clickerService;
            _uiSettings = assetProvider.GetUiSettings();
            _clickTargetImage.sprite = assetProvider.GetClickerSettings().ClickTargetSprite;
            _coinIcon.sprite = assetProvider.GetClickerSettings().CoinSprite;
            CreateFloatingRewards();
        }

        public void Show()
        {
            _clickButton.onClick.AddListener(Click);
            _wallet.Subscribe(UpdateView);
            _state.Subscribe(UpdateView);
            UpdateView();
        }

        public void UpdateView()
        {
            _coinsText.SetText($"{_wallet.Coins}");
            _clickRewardText.SetText($"+{_state.RewardPerClick} за клик");
            _incomeText.SetText($"+{_state.IncomePerSecond} в секунду");
        }

        private void CreateFloatingRewards()
        {
            for (int i = 0; i < _uiSettings.FloatingRewardPoolSize; i++)
            {
                FloatingRewardElement element = Instantiate(_uiSettings.FloatingRewardPrefab, _floatingRewardContainer);
                element.gameObject.SetActive(false);
                _floatingRewards.Add(element);
            }
        }

        private void Click()
        {
            _clickerService.Click();
            ShowFloatingReward();
            _punchVersion++;
            PunchAsync(_punchVersion, destroyCancellationToken).Forget();
        }

        private void ShowFloatingReward()
        {
            FloatingRewardElement element = _floatingRewards[_nextFloatingReward];
            _nextFloatingReward = (_nextFloatingReward + 1) % _floatingRewards.Count;
            Vector2 center = _floatingRewardContainer.rect.size * 0.5f;
            Vector2 offset = Random.insideUnitCircle * _uiSettings.FloatingRewardSpread;
            element.Play(_state.RewardPerClick, center + offset, destroyCancellationToken);
        }

        private async UniTaskVoid PunchAsync(int version, CancellationToken cancellationToken)
        {
            float elapsed = 0f;
            float duration = _uiSettings.PunchDuration;

            while (elapsed < duration && version == _punchVersion)
            {
                float scale = Mathf.Lerp(_uiSettings.PunchScale, 1f, elapsed / duration);
                _clickTargetTransform.localScale = new Vector3(scale, scale, 1f);
                elapsed += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            if (version == _punchVersion)
            {
                _clickTargetTransform.localScale = Vector3.one;
            }
        }
    }
}
