using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductsInventory", fileName = "ProductsInventory", order = 56)]
    public class ProductsInventory : ScriptableObject
    {
        [SerializeField] private ProductList _productList;

        public IReadOnlyList<Product> OpenedProducts => _productList.Products;

        public Product RandomOpenedProduct()
        {
            return OpenedProducts[Random.Range(0, OpenedProducts.Count)];
        }
    }
}