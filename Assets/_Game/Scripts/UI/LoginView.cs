using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PriceForgeClicker.UI
{
    public sealed class LoginView : MonoBehaviour
    {
        [Header("Fields")]
        [SerializeField] private TMP_InputField _emailInput;
        [SerializeField] private TMP_InputField _passwordInput;

        [Header("Buttons")]
        [SerializeField] private Button _registerButton;
        [SerializeField] private Button _loginButton;

        [Header("Feedback")]
        [SerializeField] private TMP_Text _errorText;

        public event Action OnRegisterClicked;
        public event Action OnLoginClicked;

        public string Email => _emailInput.text;

        public string Password => _passwordInput.text;

        public void Show()
        {
            _registerButton.onClick.AddListener(ClickRegister);
            _loginButton.onClick.AddListener(ClickLogin);
            ShowError(string.Empty);
            SetBusy(false);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _registerButton.onClick.RemoveListener(ClickRegister);
            _loginButton.onClick.RemoveListener(ClickLogin);
            gameObject.SetActive(false);
        }

        public void ShowError(string message) => _errorText.SetText($"{message}");

        public void SetBusy(bool isBusy)
        {
            _registerButton.interactable = !isBusy;
            _loginButton.interactable = !isBusy;
        }

        private void ClickRegister() => OnRegisterClicked?.Invoke();

        private void ClickLogin() => OnLoginClicked?.Invoke();
    }
}
