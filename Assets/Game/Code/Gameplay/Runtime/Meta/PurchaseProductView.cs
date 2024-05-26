using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class PurchaseProductView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _priceText;

        public void Render(PurchaseProduct product, string localizedName, bool opened)
        {
            _icon.sprite = product.Icon;
            _nameText.text = localizedName;
            _priceText.text = product.Price.ToPriceTag();
        }
    }
}