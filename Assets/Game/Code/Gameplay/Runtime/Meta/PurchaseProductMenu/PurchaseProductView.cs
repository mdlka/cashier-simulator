using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _button;
        [Header("Opened product settings")]
        [SerializeField] private string _openedText;
        [SerializeField] private Color _openedTextColor;

        internal event Action<PurchaseProductView> Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Render(Sprite icon, Currency price, string localizedName, bool opened)
        {
            _icon.sprite = icon;
            _nameText.text = localizedName;
            _priceText.text = opened ? _openedText : price.ToPriceTag();
            _priceText.color = opened ? _openedTextColor : _priceText.color;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke(this);
        }
    }
}