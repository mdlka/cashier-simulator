using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductBuyMenuView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _upgradeText;
        [SerializeField] private BuyButton _buyButton;
        [SerializeField] private UpgradeButton _upgradePriceButton;
        [SerializeField] private UpgradeButton _upgradePopularityButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        public event Action BuyButtonClicked;
        public event Action UpgradePriceButtonClicked;
        public event Action UpgradePopularityButtonClicked;

        public ProductInfo LastRenderedProduct { get; private set; }

        private void OnEnable()
        {
            _buyButton.Clicked += OnBuyButtonClicked;
            _upgradePriceButton.Clicked += OnUpgradePriceClicked;
            _upgradePopularityButton.Clicked += OnUpgradePopularityClicked;
        }

        private void OnDisable()
        {
            _buyButton.Clicked -= OnBuyButtonClicked;
            _upgradePriceButton.Clicked -= OnUpgradePriceClicked;
            _upgradePopularityButton.Clicked -= OnUpgradePopularityClicked;
        }

        public void Render(ProductInfo productInfo, IReadOnlyWallet wallet, bool opened)
        {
            LastRenderedProduct = productInfo;
            
            _canvasGroup.Enable();

            _icon.sprite = productInfo.Icon;
            _nameText.text = productInfo.RuName;
            
            _buyButton.Render(productInfo.PurchasePrice, wallet);
            _upgradePriceButton.Render(productInfo.PriceUpgrade, wallet, 
                new Currency(productInfo.PriceUpgrade.CurrentValue).ToPriceTag(), 
                new Currency(productInfo.PriceUpgrade.AppendValue).ToPriceTag());
            // _upgradePopularityButton.Render(productInfo.PopularityUpgrade, wallet, 
            //     productInfo.PopularityUpgrade.CurrentValue.ToString(), 
            //     productInfo.PopularityUpgrade.AppendValue.ToString());
            
            _buyButton.gameObject.SetActive(!opened);
            _upgradePriceButton.gameObject.SetActive(opened);
            _upgradePopularityButton.gameObject.SetActive(false);

            _upgradeText.enabled = opened;
        }

        public void Clear()
        {
            _canvasGroup.Disable();
        }

        private void OnBuyButtonClicked()
        {
            BuyButtonClicked?.Invoke();
        }

        private void OnUpgradePriceClicked()
        {
            UpgradePriceButtonClicked?.Invoke();
        }

        private void OnUpgradePopularityClicked()
        {
            UpgradePopularityButtonClicked?.Invoke();
        }
    }
}