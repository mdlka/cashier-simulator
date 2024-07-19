using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductBuyMenuView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _upgradeText;
        [SerializeField] private BuyButton _buyButton;
        [SerializeField] private UpgradeButton _upgradePriceButton;
        [SerializeField] private BoostButton _boostPopularityButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        public event Action BuyButtonClicked;
        public event Action UpgradePriceButtonClicked;
        public event Action BoostPopularityButtonClicked;

        public ProductInfo LastRenderedProduct { get; private set; }

        private void OnEnable()
        {
            _buyButton.Clicked += OnBuyButtonClicked;
            _upgradePriceButton.Clicked += OnUpgradePriceButtonClicked;
            _boostPopularityButton.Clicked += OnBoostPopularityButtonClicked;
        }

        private void OnDisable()
        {
            _buyButton.Clicked -= OnBuyButtonClicked;
            _upgradePriceButton.Clicked -= OnUpgradePriceButtonClicked;
            _boostPopularityButton.Clicked -= OnBoostPopularityButtonClicked;
        }

        public void Render(ProductInfo productInfo, IReadOnlyWallet wallet, bool opened)
        {
            LastRenderedProduct = productInfo;
            
            _canvasGroup.Enable();

            _icon.sprite = productInfo.Icon;
            _nameText.text = GamePlatformSdkContext.Current.Language == Language.Russian ? productInfo.RuName : productInfo.EnName;
            
            _buyButton.Render(productInfo.PurchasePrice, wallet);
            _upgradePriceButton.Render(productInfo.PriceUpgrade, wallet, 
                new Currency(productInfo.PriceUpgrade.CurrentValue).ToPriceTag(), 
                new Currency(productInfo.PriceUpgrade.NextValue).ToPriceTag());
            _boostPopularityButton.Render(productInfo.PopularityBoost);

            _buyButton.gameObject.SetActive(!opened);
            _upgradePriceButton.gameObject.SetActive(opened);
            _boostPopularityButton.gameObject.SetActive(opened);

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

        private void OnUpgradePriceButtonClicked()
        {
            UpgradePriceButtonClicked?.Invoke();
        }

        private void OnBoostPopularityButtonClicked()
        {
            BoostPopularityButtonClicked?.Invoke();
        }
    }
}