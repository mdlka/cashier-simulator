using System.Collections;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay;

namespace YellowSquad.CashierSimulator.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Shop _shop;
        [SerializeField] private ShopDaySettings _shopDaySettings;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
        }

        private IEnumerator Start()
        {
            int day = 1;
            
            while (true)
            {
                _shop.StartDay(_shopDaySettings);
                
                Debug.Log($"Day {day} started");
            
                yield return new WaitUntil(() => _shop.WorkIsDone);
                yield return new WaitForSeconds(5);
                
                Debug.Log($"Day {day++} ended");
            }
        }
    }
}
