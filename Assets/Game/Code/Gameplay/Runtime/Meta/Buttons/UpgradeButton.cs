using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class UpgradeButton : MonoBehaviour
    {
        private const string MaxUpgradesText = "Макс";
        
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _statsText;
        [SerializeField] private Button _button;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _cantBuyColor;
        [SerializeField] private string _statsHeader;
        [SerializeField] private string _defaultText;
        [SerializeField] private string _cantBuyText;

        private string _defaultColorHtml;

        public event Action Clicked;

        private void Awake()
        {
            _defaultColorHtml = "#" + ColorUtility.ToHtmlStringRGB(_defaultColor);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Render(BaseUpgrade upgrade, IReadOnlyWallet wallet, string currentValue, string nextValue)
        {
            if (upgrade.Max)
            {
                _statsText.text = $"{StatsHeader(upgrade.CurrentLevel)}\n<size=80%>{currentValue}";
                _priceText.text = MaxUpgradesText;
                _button.image.color = _defaultColor;
                _button.interactable = false;
            }
            else
            {
                bool canBuy = wallet.CanSpend(upgrade.Price);
            
                _statsText.text = $"{StatsHeader(upgrade.CurrentLevel)}\n<size=80%>{currentValue} -> <color={_defaultColorHtml}>{nextValue}</color>";
                _priceText.text = canBuy ? $"{upgrade.Price.ToPriceTag()}\n{_defaultText}" : $"{upgrade.Price.ToPriceTag()}\n{_cantBuyText}";
                _button.image.color = canBuy ? _defaultColor : _cantBuyColor;
                _button.interactable = true;
            }
        }

        private string StatsHeader(long currentLevel)
        {
            return currentLevel == 0 ? _statsHeader : $"{_statsHeader} <b>({currentLevel})</b>";
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }
    }
}