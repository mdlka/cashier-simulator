using System.Collections;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay;
using YellowSquad.CashierSimulator.Gameplay.Meta;
using YellowSquad.CashierSimulator.UserInput;

namespace YellowSquad.CashierSimulator.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Shop _shop;
        [SerializeField] private ShopStats _shopStats;
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
            int day = 1;
            _needUpdate = true;
            
            _productsInventory.Initialize();
            _shopStats.Initialize();
            
            while (true)
            {
                _inputRouter.ResetCameraRotation();
                _shop.StartDay();
                _shopStats.StartDay();
                
                Debug.Log($"Day {day} started");
            
                yield return new WaitUntil(() => _shop.WorkIsDone);
                yield return new WaitForSeconds(5);
                
                Debug.Log($"Day {day++} ended");

                _needUpdate = false;
                _inputRouter.SetActiveCursor(true);
                _shopStats.SaveCurrentDay();

                _purchaseProductMenu.Open();

                yield return new WaitUntil(() => _purchaseProductMenu.Opened == false);
                
                _shopUpgradeMenu.Open();
                
                yield return new WaitUntil(() => _shopUpgradeMenu.Opened == false);
                
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
