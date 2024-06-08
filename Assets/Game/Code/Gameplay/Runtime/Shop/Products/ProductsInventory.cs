using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductsInventory", fileName = "ProductsInventory", order = 56)]
    public class ProductsInventory : ScriptableObject
    {
        [NonSerialized] private readonly List<Product> _openedProducts = new();

        [SerializeField] private ProductList _productList;
        [SerializeField] private Product _startOpenedProduct;

        public IReadOnlyList<Product> OpenedProducts => _openedProducts;

        public void Initialize()
        {
            _openedProducts.Add(_startOpenedProduct);
        }

        public void Add(Product product)
        {
            if (_openedProducts.Contains(product))
                throw new InvalidOperationException();
            
            _openedProducts.Add(product);
        }

        public Product RandomOpenedProduct()
        {
            return OpenedProducts[Random.Range(0, OpenedProducts.Count)];
        }
    }
}