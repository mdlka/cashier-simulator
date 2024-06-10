using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductList", fileName = "ProductList", order = 56)]
    public class ProductList : ScriptableObject
    {
        [SerializeField] private ProductInfo[] _products;

        [NonSerialized] private List<Product> _productsList;

        public IReadOnlyList<Product> Products => _productsList ??= _products.Select(product => product.Product).ToList();

        private void OnValidate()
        {
            for (int i = 0; i < _products.Length; i++)
            {
                for (int j = 0; j < _products.Length; j++)
                {
                    if (i == j)
                        continue;
                    
                    if (_products[i] == null || _products[j] == null)
                        continue;

                    if (_products[i].Product != _products[j].Product)
                        continue;

                    _products[j] = null;
                }
            }
        }

        internal ProductInfo FindInfoBy(string tag)
        {
            return _products.First(info => info.Product.NameTag == tag);
        }
    }
}