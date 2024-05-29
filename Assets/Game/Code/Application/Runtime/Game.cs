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
        [SerializeField] private ShopDaySettings _shopDaySettings;
        [SerializeField] private PurchaseProductMenu _purchaseProductMenu;
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
            
            while (true)
            {
                _shop.StartDay(_shopDaySettings);
                
                Debug.Log($"Day {day} started");
            
                yield return new WaitUntil(() => _shop.WorkIsDone);
                yield return new WaitForSeconds(5);
                
                Debug.Log($"Day {day++} ended");

                _needUpdate = false;
                _inputRouter.SetActiveCursor(true);
                
                _purchaseProductMenu.Open();

                yield return new WaitUntil(() => _purchaseProductMenu.Opened == false);
                
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
