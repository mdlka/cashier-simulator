using System;
using System.Linq;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create ProductsNames", fileName = "ProductsNames", order = 56)]
    public class ProductsNames : ScriptableObject
    {
        [SerializeField] private ProductName[] _names;

        private void OnValidate()
        {
            for (int i = 0; i < _names.Length; i++)
            {
                for (int j = 0; j < _names.Length; j++)
                {
                    if (i == j)
                        continue;
                    
                    if (_names[i].Product != _names[j].Product)
                        continue;

                    _names[j] = null;
                }
            }
        }

        public string RuName(string productTag)
        {
            return _names.First(p => p.Product.NameTag == productTag).RuName;
        }
        
        [Serializable]
        private class ProductName
        {
            [field: SerializeField] public Product Product { get; private set; }
            [field: SerializeField] public string RuName { get; private set; }
        }
    }
}