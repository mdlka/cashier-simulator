using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class ScannedProductPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _totalText;
        
        public string TargetProductNameTag { get; private set; }
        public int CurrentProductsCount { get; private set; }
        
        internal void Render(string productNameTag, Currency productPrice, int count = 1)
        {
            TargetProductNameTag = productNameTag;
            CurrentProductsCount = count;

            _nameText.text = productNameTag;
            _countText.text = count.ToString();
            _priceText.text = productPrice.ToPriceTag();
            _totalText.text = (productPrice * count).ToPriceTag();
        }
    }
}