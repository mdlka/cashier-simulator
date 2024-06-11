﻿using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [Serializable]
    internal class ProductInfo
    {
        [SerializeField] private Product _product;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _ruName;
        [SerializeField, Min(0)] private long _purchasePriceInCents;
        [SerializeField] private ProductUpgrade _priceUpgrade;
        [SerializeField] private ProductUpgrade _popularityUpgrade;

        private Currency? _purchasePrice;

        public Product Product => _product;
        public Sprite Icon => _icon;
        public string RuName => _ruName;
        public Currency Price => _priceUpgrade.CurrentValue;
        public Currency PurchasePrice => _purchasePrice ??= new Currency(_purchasePriceInCents);
        public ProductUpgrade PriceUpgrade => _priceUpgrade;
        public ProductUpgrade PopularityUpgrade => _popularityUpgrade;
    }
}