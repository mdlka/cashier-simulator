using System.Linq;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    public class PurchaseProductMenu : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PurchaseProductListView _productListView;
        [SerializeField] private PurchaseProductBuyMenuView _productBuyMenuView;
        [SerializeField] private ProductsInventory _productsInventory;

        public bool Opened { get; private set; }

        private void Awake()
        {
            _productListView.Close(duration: 0);

            _productBuyMenuView.BuyButtonClicked += OnBuyButtonClicked;
        }

        public void Open()
        {
            Opened = true;

            _productBuyMenuView.Clear();
            _productListView.Render(_productsInventory.OpenedProducts, 
                onCloseButtonClick: () =>
                {
                    _productListView.Close();
                    Opened = false;
                },
                onProductSelect: productInfo =>
                {
                    _productBuyMenuView.Render(productInfo, _wallet.CurrentValue, 
                        _productsInventory.OpenedProducts.Contains(productInfo.Product));
                });
        }

        private void OnBuyButtonClicked()
        {
            var targetProduct = _productBuyMenuView.LastRenderedProduct;

            if (_productsInventory.OpenedProducts.Contains(targetProduct.Product))
                return;
            
            if (_wallet.CanSpend(targetProduct.PurchasePrice) == false)
                return;
            
            _productsInventory.Add(targetProduct.Product);
            _wallet.Spend(targetProduct.PurchasePrice);
            
            _productListView.UpdateRender(targetProduct, opened: true);
            _productBuyMenuView.Render(targetProduct, _wallet.CurrentValue, opened: true);
        }

        private void OnDestroy()
        {
            _productBuyMenuView.BuyButtonClicked -= OnBuyButtonClicked;
        }
    }
}
