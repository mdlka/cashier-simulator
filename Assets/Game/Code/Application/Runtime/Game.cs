using System.Collections;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay;
using YellowSquad.CashierSimulator.Gameplay.Meta;
using YellowSquad.CashierSimulator.UserInput;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Shop _shop;
        [SerializeField] private ProductsInventory _productsInventory;
        [SerializeField] private PurchaseProductMenu _purchaseProductMenu;
        [SerializeField] private ShopUpgradeMenu _shopUpgradeMenu;
        [SerializeField] private InputRouter _inputRouter;

        private bool _needUpdate = true;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
        }

        private IEnumerator Start()
        {
            yield return GamePlatformSdkContext.Current.Initialize();
            
            _needUpdate = true;
            
            _productsInventory.Initialize(GamePlatformSdkContext.Current.Save);
            _shop.Initialize(GamePlatformSdkContext.Current.Save);
            
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
                yield return new WaitUntil(() => _shop.StatsShowing == false);

                GamePlatformSdkContext.Current.Save.Save();
                
                yield return GamePlatformSdkContext.Current.Advertisement.ShowInterstitial();

                _purchaseProductMenu.Open();
                yield return new WaitUntil(() => _purchaseProductMenu.Opened == false);
                _shop.SaveProducts();
                
                _shopUpgradeMenu.Open();
                yield return new WaitUntil(() => _shopUpgradeMenu.Opened == false);
                
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
    }
}
