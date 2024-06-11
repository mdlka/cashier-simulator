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

        public bool Opened { get; private set; }
        
        private void Awake()
        {
            _canvasGroup.Disable();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        public void Open()
        {
            Opened = true;
            _canvasGroup.Enable();
        }

        private void OnCloseButtonClick()
        {
            Opened = false;
            _canvasGroup.Disable(0.2f);
        }
    }
}
