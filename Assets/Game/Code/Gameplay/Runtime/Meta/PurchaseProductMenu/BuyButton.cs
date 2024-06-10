using System;
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
        [SerializeField] private string _defaultText;
        [SerializeField] private string _cantBuyText;

        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Render(Currency price, Currency currentBalance)
        {
            bool canBuy = currentBalance.TotalCents > price.TotalCents;
            
            _priceText.text = canBuy ? $"{price.ToPriceTag()}\n{_defaultText}" : _cantBuyText;
            _button.image.color = canBuy ? _defaultColor : _cantBuyColor;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }
    }
}