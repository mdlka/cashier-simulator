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
            _priceText.text = price.ToPriceTag();
            _button.image.color = currentBalance.TotalCents > price.TotalCents ? _defaultColor : _cantBuyColor;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }
    }
}