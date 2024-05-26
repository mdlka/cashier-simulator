using System;
using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    public class PurchaseProductMenu : MonoBehaviour
    {
        [SerializeField] private PurchaseProductMenuView _view;
        [SerializeField] private ProductsInventory _productsInventory;
        [SerializeField] private List<PurchaseProduct> _products;

        public bool Opened { get; private set; }

        private void Awake()
        {
            _view.Close();
        }

        public void Open()
        {
            Opened = true;
            
            _view.Render(_products, _productsInventory.OpenedProducts, 
                onCloseButtonClick: () =>
                {
                    _view.Close();
                    Opened = false;
                });
        }
    }
    
    [Serializable]
    internal class PurchaseProduct
    {
        [SerializeField] private Product _product;
        [SerializeField] private Sprite _icon;
        [SerializeField, Min(0)] private long _priceInCents;

        private Currency? _currency;
        
        public string NameTag => _product.NameTag;
        public Sprite Icon => _icon;
        public Currency Price => _currency ??= new Currency(_priceInCents);
    }
}
