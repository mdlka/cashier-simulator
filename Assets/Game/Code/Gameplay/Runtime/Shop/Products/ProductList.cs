using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductList", fileName = "ProductList", order = 56)]
    public class ProductList : ScriptableObject
    {
        [SerializeField] private ProductInfo[] _products;

        [NonSerialized] private List<Product> _productsList;

        private ISave _save;

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

        internal void Initialize(ISave save)
        {
            _save = save;

            if (_save.HasKey(SaveConstants.ProductsLevelsSaveKey))
            {
                var levels = JsonConvert.DeserializeObject<Dictionary<string, long>>(
                        _save.GetString(SaveConstants.ProductsLevelsSaveKey));

                foreach (var product in _products)
                {
                    if (levels.ContainsKey(product.Product.NameTag))
                        product.PriceUpgrade.Initialize(levels[product.Product.NameTag]);
                    else
                        product.PriceUpgrade.Initialize();
                }
            }
            else
            {
                foreach (var product in _products)
                    product.PriceUpgrade.Initialize();
            }
        }

        internal void Save()
        {
            var levels = new Dictionary<string, long>();

            foreach (var product in _products)
                levels.TryAdd(product.Product.NameTag, product.PriceUpgrade.CurrentLevel);

            _save.SetString(SaveConstants.ProductsLevelsSaveKey, JsonConvert.SerializeObject(levels));
        }

        internal void DeactivateBoosts()
        {
            foreach (var product in _products)
                product.PopularityBoost.Deactivate();
        }

        internal ProductInfo FindInfoBy(string tag)
        {
            return _products.First(info => info.Product.NameTag == tag);
        }

        internal ProductInfo[] FindInfoBy(IReadOnlyCollection<string> tags)
        {
            return _products.Where(info => tags.Contains(info.Product.NameTag)).ToArray();
        }
    }
}