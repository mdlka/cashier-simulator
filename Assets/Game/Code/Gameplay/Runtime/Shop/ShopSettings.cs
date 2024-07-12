using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ShopSettings", fileName = "ShopSettings", order = 56)]
    public class ShopSettings : ScriptableObject
    {
        private readonly Boost _popularityBoost = new();
        private readonly Boost _productsPriceBoost = new();

        [SerializeField, Min(0)] private int _minCostumersPerDay;
        [SerializeField, Min(0)] private int _maxCostumersPerHour;
        [SerializeField, Min(1f)] private float _startTimeSpeed;
        [SerializeField, Min(1f)] private float _endTimeSpeed;
        [SerializeField] private CustomerProductListFactory _productListFactory;
        [SerializeField] private ShopUpgrade _popularityUpgrade;
        [SerializeField] private ShopUpgrade _cartCapacityUpgrade;

        public float TimeSpeed => Mathf.Lerp(_startTimeSpeed, _endTimeSpeed, Popularity) * (_popularityBoost.Active ? 0.75f : 1f);
        public int MinCostumersPerDay => _minCostumersPerDay;
        public int MaxCostumersPerHour => _popularityBoost.Active ? _maxCostumersPerHour * 2 : _maxCostumersPerHour;
        public long ProductsPriceFactor => _productsPriceBoost.Active ? 2 : 1;
        public float Popularity => PopularityUpgrade.CurrentValue / 100f;
        public int MaxCartCapacity => (int)CartCapacityUpgrade.CurrentValue;
        public CustomerProductListFactory ProductListFactory => _productListFactory;

        internal ShopUpgrade PopularityUpgrade => _popularityUpgrade;
        internal ShopUpgrade CartCapacityUpgrade => _cartCapacityUpgrade;
        internal Boost PopularityBoost => _popularityBoost;
        internal Boost ProductsPriceBoost => _productsPriceBoost;
    }
}