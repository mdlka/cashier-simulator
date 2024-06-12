using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    public class ShopUpgradeMenu : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private UpgradeButton _upgradeCartButton;
        [SerializeField] private UpgradeButton _upgradePopularityButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ShopSettings _shopSettings;

        public bool Opened { get; private set; }
        
        private void Awake()
        {
            _canvasGroup.Disable();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _upgradeCartButton.Clicked += OnUpgradeCartButtonClicked;
            _upgradePopularityButton.Clicked += OnUpgradePopularityButtonClicked;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
            _upgradeCartButton.Clicked -= OnUpgradeCartButtonClicked;
            _upgradePopularityButton.Clicked -= OnUpgradePopularityButtonClicked;
        }

        public void Open()
        {
            Opened = true;
            _canvasGroup.Enable();
            
            RenderUpgradeButton(_upgradeCartButton, _shopSettings.CartCapacityUpgrade);
            RenderUpgradeButton(_upgradePopularityButton, _shopSettings.PopularityUpgrade, "%");
        }

        private void OnCloseButtonClick()
        {
            Opened = false;
            _canvasGroup.Disable(0.2f);
        }

        private void OnUpgradeCartButtonClicked()
        {
            ApplyUpgrade(_shopSettings.CartCapacityUpgrade);
            RenderUpgradeButton(_upgradeCartButton, _shopSettings.CartCapacityUpgrade);
        }

        private void OnUpgradePopularityButtonClicked()
        {
            ApplyUpgrade(_shopSettings.PopularityUpgrade);
            RenderUpgradeButton(_upgradePopularityButton, _shopSettings.PopularityUpgrade, "%");
        }

        private void ApplyUpgrade(ShopUpgrade upgrade)
        {
            if (_wallet.CanSpend(upgrade.Price) == false)
                return;

            _wallet.Spend(upgrade.Price);
            upgrade.Upgrade();
        }

        private void RenderUpgradeButton(UpgradeButton button, ShopUpgrade upgrade, string endSymbols = "")
        {
            button.Render(upgrade, _wallet, $"{upgrade.CurrentValue}{endSymbols}", $"{upgrade.AppendValue}{endSymbols}");
        }
    }
}
