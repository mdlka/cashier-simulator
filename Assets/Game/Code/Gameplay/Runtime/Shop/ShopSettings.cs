using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopSettings", fileName = "ShopSettings", order = 56)]
    public class ShopSettings : ScriptableObject
    {
        [field: SerializeField, Range(0f, 1f)] public float CreateCostumerChance { get; private set; }
        [field: SerializeField] public int MaxCostumersPerHour { get; private set; }
        [field: SerializeField, Min(1)] public int MaxCartCapacity { get; private set; }
        [field: SerializeField, Min(1)] public float TimeSpeed { get; private set; }
        [field: SerializeField] public CustomerProductListFactory ProductListFactory { get; private set; }
    }
}