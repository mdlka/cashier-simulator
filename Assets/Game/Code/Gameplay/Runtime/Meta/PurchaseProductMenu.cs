using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    public class PurchaseProductMenu : MonoBehaviour
    {
        [SerializeField] private PurchaseProductMenuView _view;
        [SerializeField] private ProductsInventory _productsInventory;

        public bool Opened { get; private set; }

        private void Awake()
        {
            _view.Close();
        }

        public void Open()
        {
            Opened = true;
            
            _view.Render(_productsInventory.OpenedProducts, 
                onCloseButtonClick: () =>
                {
                    _view.Close();
                    Opened = false;
                });
        }
    }
}
