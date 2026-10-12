using UnityEngine;

namespace PriceForgeClicker.Settings
{
    [CreateAssetMenu(menuName = "SO/Clicker Settings")]
    public sealed class ClickerSettings : ScriptableObject
    {
        [Header("Economy")]
        [SerializeField] private long _baseClickReward = 1;
        [SerializeField] private long _startCoins;

        [Header("Visual")]
        [SerializeField] private Sprite _clickTargetSprite;
        [SerializeField] private Sprite _coinSprite;

        public long BaseClickReward => _baseClickReward;
        public long StartCoins => _startCoins;
        public Sprite ClickTargetSprite => _clickTargetSprite;
        public Sprite CoinSprite => _coinSprite;
    }
}
