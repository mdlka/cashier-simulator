using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopDaySettings", fileName = "ShopDaySettings", order = 56)]
    public class ShopDaySettings : ScriptableObject
    {
        [field: SerializeField] public int CostumersCount { get; private set; }
        [field: SerializeField] public ProductListFactory ProductListFactory { get; private set; }
    }
}