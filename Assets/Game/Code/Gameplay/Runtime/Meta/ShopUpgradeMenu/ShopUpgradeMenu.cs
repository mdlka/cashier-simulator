using System;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    public class ShopUpgradeMenu : MonoBehaviour
    {
        [SerializeField] private PurchaseProductMenu _purchaseProductMenu;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private UpgradeButton _upgradeCartButton;
        [SerializeField] private UpgradeButton _upgradePopularityButton;
        [SerializeField] private BoostButton _boostProductsPriceButton;
        [SerializeField] private BoostButton _boostPopularityButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ShopSettings _shopSettings;

        public bool Opened { get; private set; }
        
        private void Awake()
        {
            _canvasGroup.Disable();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
            _backButton.onClick.AddListener(OpenProductsMenu);
            _upgradeCartButton.Clicked += OnUpgradeCartButtonClicked;
            _upgradePopularityButton.Clicked += OnUpgradePopularityButtonClicked;
            _boostProductsPriceButton.Clicked += OnBoostProductsPriceButtonClicked;
            _boostPopularityButton.Clicked += OnBoostPopularityButtonClicked;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
            _backButton.onClick.RemoveListener(OpenProductsMenu);
            _upgradeCartButton.Clicked -= OnUpgradeCartButtonClicked;
            _upgradePopularityButton.Clicked -= OnUpgradePopularityButtonClicked;
            _boostProductsPriceButton.Clicked -= OnBoostProductsPriceButtonClicked;
            _boostPopularityButton.Clicked -= OnBoostPopularityButtonClicked;
        }

        public void Open(float openDuration = 0.2f)
        {
            Opened = true;
            
            RenderUpgradeButton(_upgradeCartButton, _shopSettings.CartCapacityUpgrade);
            RenderUpgradeButton(_upgradePopularityButton, _shopSettings.PopularityUpgrade, "%");
            _boostProductsPriceButton.Render(_shopSettings.ProductsPriceBoost);
            _boostPopularityButton.Render(_shopSettings.PopularityBoost);
            
            _canvasGroup.Enable(openDuration);
        }

        private void Close()
        {
            Opened = false;
            _canvasGroup.Disable(0.2f);
        }

        private void OpenProductsMenu()
        {
            Close();
            _purchaseProductMenu.Open(openDuration: 0f);
        }

        private void OnBoostProductsPriceButtonClicked()
        {
            ApplyBoost(_shopSettings.ProductsPriceBoost, 
                onEnd: () => _boostProductsPriceButton.Render(_shopSettings.ProductsPriceBoost));
        }

        private void OnBoostPopularityButtonClicked()
        {
            ApplyBoost(_shopSettings.PopularityBoost, 
                onEnd: () => _boostPopularityButton.Render(_shopSettings.PopularityBoost));
        }

        private void OnUpgradeCartButtonClicked()
        {
            ApplyUpgrade(_shopSettings.CartCapacityUpgrade);
            
            RenderUpgradeButton(_upgradeCartButton, _shopSettings.CartCapacityUpgrade);
            RenderUpgradeButton(_upgradePopularityButton, _shopSettings.PopularityUpgrade, "%");
        }

        private void OnUpgradePopularityButtonClicked()
        {
            ApplyUpgrade(_shopSettings.PopularityUpgrade);
            
            RenderUpgradeButton(_upgradeCartButton, _shopSettings.CartCapacityUpgrade);
            RenderUpgradeButton(_upgradePopularityButton, _shopSettings.PopularityUpgrade, "%");
        }

        private void ApplyUpgrade(ShopUpgrade upgrade)
        {
            if (_wallet.CanSpend(upgrade.Price) == false)
                return;

            _wallet.Spend(upgrade.Price);
            upgrade.Upgrade();
        }

        private void ApplyBoost(Boost boost, Action onEnd)
        {
            if (boost.Active)
                return;

            GamePlatformSdkContext.Current.Advertisement.ShowRewarded(onEnd: result =>
            {
                if (result == Result.Success)
                    boost.Activate();
                
                onEnd?.Invoke();
            });
        }

        private void RenderUpgradeButton(UpgradeButton button, ShopUpgrade upgrade, string endSymbols = "")
        {
            button.Render(upgrade, _wallet, $"{upgrade.CurrentValue}{endSymbols}", $"{upgrade.NextValue}{endSymbols}");
        }
    }
}
