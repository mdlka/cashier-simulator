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
        
        [Serializable]
        internal class ProductInfo
        {
            [SerializeField] private Product _product;
            [SerializeField] private Sprite _icon;
            [SerializeField] private string _ruName;
            [SerializeField, Min(0)] private long _priceInCents;
            [SerializeField, Min(0)] private long _purchasePriceInCents;

            private Currency? _price;
            private Currency? _purchasePrice;

            public Product Product => _product;
            public Sprite Icon => _icon;
            public string RuName => _ruName;
            public Currency Price => _price ??= new Currency(_priceInCents);
            public Currency PurchasePrice => _purchasePrice ??= new Currency(_purchasePriceInCents);
        }
    }
}