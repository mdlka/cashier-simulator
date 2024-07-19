using System;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _upgradeIcon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _button;
        [Header("Opened product settings")]
        [SerializeField, LeanTranslationName] private string _openedTextTranslationName;
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

            string localizedOpenedText = LeanLocalization.GetTranslationText(_openedTextTranslationName);
            
            _icon.sprite = info.Icon;
            _upgradeIcon.enabled = opened;
            _nameText.text = GamePlatformSdkContext.Current.Language == Language.Russian ? info.RuName : info.EnName;
            _priceText.text = opened ? localizedOpenedText : info.PurchasePrice.ToPriceTag();
            _priceText.color = opened ? _openedTextColor : _priceText.color;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke(this);
        }
    }
}