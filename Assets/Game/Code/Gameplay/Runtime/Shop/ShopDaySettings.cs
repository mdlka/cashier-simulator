using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopDaySettings", fileName = "ShopDaySettings", order = 56)]
    public class ShopDaySettings : ScriptableObject
    {
        [field: SerializeField, Min(1)] public float TimeSpeed { get; private set; }
        [field: SerializeField] public int MaxCostumersPerHour { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float CreateCostumerChance { get; private set; }
        [field: SerializeField] public CustomerProductListFactory ProductListFactory { get; private set; }
    }
}