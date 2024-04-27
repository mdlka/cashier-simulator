using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashRegisterMonitor : MonoBehaviour
    {
        [SerializeField] private TMP_Text _givenCashText;
        [SerializeField] private TMP_Text _productsPriceText;
        [SerializeField] private TMP_Text _targetChangeText;
        [SerializeField] private TMP_Text _currentChangeText;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _wrongColor;

        public void UpdateInfo()
        {
            UpdateInfo(Currency.Zero, Currency.Zero, Currency.Zero);
        }

        public void UpdateInfo(Currency givenCash, Currency productsPrice, Currency currentChange)
        {
            _givenCashText.text = givenCash.ToPriceTag();
            _productsPriceText.text = productsPrice.ToPriceTag();

            var targetChange = givenCash - productsPrice;
            
            _targetChangeText.text = targetChange.ToPriceTag();
            _currentChangeText.text = targetChange.ToPriceTag();

            _currentChangeText.color = targetChange == currentChange ? _correctColor : _wrongColor;
        }
    }
}