using System;
using System.Linq;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

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
            _productBuyMenuView.UpgradePriceButtonClicked += OnUpgradePriceButtonClicked;
            _productBuyMenuView.BoostPopularityButtonClicked += OnBoostPopularityButtonClicked;
        }
        
        private void OnDestroy()
        {
            _productBuyMenuView.BuyButtonClicked -= OnBuyButtonClicked;
            _productBuyMenuView.UpgradePriceButtonClicked -= OnUpgradePriceButtonClicked;
            _productBuyMenuView.BoostPopularityButtonClicked -= OnBoostPopularityButtonClicked;
        }

        public void Open(float openDuration = 0.2f)
        {
            if (Opened)
                throw new InvalidOperationException("Already opened");
            
            Opened = true;

            _productBuyMenuView.Clear();
            _productListView.Render(_productsInventory.OpenedProducts, 
                onCloseButtonClick: () =>
                {
                    _productListView.Close(0f);
                    Opened = false;
                },
                onProductSelect: productInfo =>
                {
                    _productBuyMenuView.Render(productInfo, _wallet, 
                        _productsInventory.OpenedProducts.Contains(productInfo.Product.NameTag));
                }, openDuration);
        }

        private void OnBuyButtonClicked()
        {
            var targetProduct = _productBuyMenuView.LastRenderedProduct;

            if (_productsInventory.OpenedProducts.Contains(targetProduct.Product.NameTag))
                return;
            
            if (_wallet.CanSpend(targetProduct.PurchasePrice) == false)
                return;
            
            _productsInventory.Add(targetProduct.Product);
            _wallet.Spend(targetProduct.PurchasePrice);
            
            _productListView.UpdateRender(targetProduct, opened: true);
            _productBuyMenuView.Render(targetProduct, _wallet, opened: true);
        }

        private void OnBoostPopularityButtonClicked()
        {
            var targetProduct = _productBuyMenuView.LastRenderedProduct;

            if (targetProduct.PopularityBoost.Active)
                return;

            GamePlatformSdkContext.Current.Advertisement.ShowRewarded(onEnd: result =>
            {
                if (result == Result.Success)
                    targetProduct.PopularityBoost.Activate();
                
                _productBuyMenuView.Render(targetProduct, _wallet, opened: true);
            });
        }

        private void OnUpgradePriceButtonClicked()
        {
            var targetProduct = _productBuyMenuView.LastRenderedProduct;
            ApplyUpgradeProduct(targetProduct, targetProduct.PriceUpgrade);
        }

        private void ApplyUpgradeProduct(ProductInfo targetProduct, ProductUpgrade upgrade)
        {
            if (_wallet.CanSpend(upgrade.Price) == false)
                return;
            
            _wallet.Spend(upgrade.Price);
            upgrade.Upgrade();
            _productBuyMenuView.Render(targetProduct, _wallet, opened: true);
        }
    }
}
