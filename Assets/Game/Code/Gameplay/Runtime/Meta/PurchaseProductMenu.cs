using System;
using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class PurchaseProductMenu : MonoBehaviour
    {
        [SerializeField] private ProductsInventory _productsInventory;
        [SerializeField] private List<Pair> _products;

        [Serializable]
        private class Pair
        {
            [field: SerializeField] public Product Product { get; private set; }
            [field: SerializeField] public int PriceInCents { get; private set; } 
        }
    }
}
