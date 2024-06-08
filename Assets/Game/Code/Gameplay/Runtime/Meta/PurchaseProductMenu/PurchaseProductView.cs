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
        internal ProductInfo ProductInfo { get; private set; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Render(ProductInfo info, bool opened)
        {
            ProductInfo = info;
            
            _icon.sprite = info.Icon;
            _nameText.text = info.RuName;
            _priceText.text = opened ? _openedText : info.PurchasePrice.ToPriceTag();
            _priceText.color = opened ? _openedTextColor : _priceText.color;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke(this);
        }
    }
}