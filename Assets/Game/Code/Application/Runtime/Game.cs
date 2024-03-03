using System.Collections;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay;

namespace YellowSquad.CashierSimulator.Application
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Shop _shop;
        [SerializeField] private ShopDaySettings _shopDaySettings;

        private IEnumerator Start()
        {
            while (true)
            {
                _shop.StartDay(_shopDaySettings);

                yield return new WaitUntil(() => _shop.WorkIsDone);
                yield return new WaitForSeconds(5);
                
                Debug.Log("Day ended");
            }
        }
    }
}
