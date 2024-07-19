using System;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class BuyButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _cantBuyColor;
        [SerializeField, LeanTranslationName] private string _defaultTextTranslationName;
        [SerializeField, LeanTranslationName] private string _cantBuyTextTranslationName;

        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Render(Currency price, IReadOnlyWallet wallet)
        {
            bool canBuy = wallet.CanSpend(price);

            string localizedDefaultText = LeanLocalization.GetTranslationText(_defaultTextTranslationName);
            string localizedCantBuyText = LeanLocalization.GetTranslationText(_cantBuyTextTranslationName);

            _priceText.text = canBuy ? $"{price.ToPriceTag()}\n<b>{localizedDefaultText}" : $"<b>{localizedCantBuyText}";
            _button.image.color = canBuy ? _defaultColor : _cantBuyColor;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }
    }
}