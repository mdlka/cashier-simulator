using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductsInventory", fileName = "ProductsInventory", order = 56)]
    public class ProductsInventory : ScriptableObject
    {
        [SerializeField] private Product[] _products;

        public Product RandomOpenedProduct()
        {
            return _products[Random.Range(0, _products.Length)];
        }
    }
}