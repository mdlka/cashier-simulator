using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopSettings", fileName = "ShopSettings", order = 56)]
    public class ShopSettings : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int MaxCostumersPerHour { get; private set; }
        [field: SerializeField, Min(1)] public float TimeSpeed { get; private set; }
        [field: SerializeField] public CustomerProductListFactory ProductListFactory { get; private set; }
        [field: SerializeField] internal ShopUpgrade PopularityUpgrade { get; private set; }
        [field: SerializeField] internal ShopUpgrade CartCapacityUpgrade { get; private set; }

        public float Popularity => PopularityUpgrade.CurrentValue / 100f;
        public int MaxCartCapacity => (int)CartCapacityUpgrade.CurrentValue;
    }
}