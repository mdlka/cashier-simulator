using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay;
using YellowSquad.CashierSimulator.Gameplay.Meta;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.CashierSimulator.UserInput;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Application
{
    public class Game : MonoBehaviour
    {
        private const string LeaderboardName = "MainLeaderboard";

        [SerializeField] private Shop _shop;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private ProductsInventory _productsInventory;
        [SerializeField] private PurchaseProductMenu _purchaseProductMenu;
        [SerializeField] private ShopUpgradeMenu _shopUpgradeMenu;
        [SerializeField] private InputRouter _inputRouter;
        [SerializeField] private CanvasGroup _blackScreenCanvasGroup;

        private bool _needUpdate = true;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
        }

        private IEnumerator Start()
        {
            _blackScreenCanvasGroup.Enable();
            
            yield return GamePlatformSdkContext.Current.Initialize();
            
            _blackScreenCanvasGroup.Disable(0.5f);
            
            _needUpdate = true;
            
            _productsInventory.Initialize(GamePlatformSdkContext.Current.Save);
            _shop.Initialize(GamePlatformSdkContext.Current.Save);
            _wallet.Initialize(GamePlatformSdkContext.Current.Save);
            
            while (true)
            {
                _inputRouter.ResetCameraRotation();
                _shop.StartDay();
                
                yield return new WaitUntil(() => _shop.WorkIsDone);
                yield return new WaitForSeconds(5);
                
                _needUpdate = false;
                _inputRouter.SetActiveCursor(true);
                
                _shop.DeactivateBoosts();
                
                _shop.ShowStats();
                UpdateLeaderboardScore();
                GamePlatformSdkContext.Current.Save.Save();
                
                yield return new WaitUntil(() => _shop.StatsShowing == false);
                yield return GamePlatformSdkContext.Current.Advertisement.ShowInterstitial();

                _purchaseProductMenu.Open();
                
                yield return new WaitForSeconds(0.3f);
                _inputRouter.ResetCameraRotation();
                
                yield return new WaitUntil(() => _purchaseProductMenu.Opened == false);
                _shop.SaveProducts();
                
                _shopUpgradeMenu.Open();
                yield return new WaitUntil(() => _shopUpgradeMenu.Opened == false);
                _shop.SaveShopUpgrades();
                
                GamePlatformSdkContext.Current.Save.Save();
                
                _needUpdate = true;
            }
        }

        private void Update()
        {
            if (_needUpdate == false)
                return;
            
            _inputRouter.UpdateInput();
        }

        private void UpdateLeaderboardScore()
        {
            long bestScore = GamePlatformSdkContext.Current.Save.GetLeaderboardScore(LeaderboardName);
            long currentScore = _wallet.CurrentValue.TotalCents;

            if (currentScore <= bestScore)
                return;
            
            GamePlatformSdkContext.Current.Save.SetLeaderboardScore(LeaderboardName, currentScore);
        }
    }
}
