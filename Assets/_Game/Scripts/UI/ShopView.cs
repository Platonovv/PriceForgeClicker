using TMPro;
using UnityEngine;

namespace PriceForgeClicker.UI
{
    public sealed class ShopView : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Transform _content;
        [SerializeField] private TMP_Text _statusText;

        public Transform Content => _content;

        public void ShowStatus(string message)
        {
            _statusText.gameObject.SetActive(true);
            _statusText.SetText(message);
        }

        public void HideStatus() => _statusText.gameObject.SetActive(false);
    }
}
