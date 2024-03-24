using System;
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

        public void UpdateInfo(float givenCash = 0f, float productsPrice = 0f, float currentChange = 0f)
        {
            _givenCashText.text = FormatCashText(givenCash);
            _productsPriceText.text = FormatCashText(productsPrice);

            float targetChange = givenCash - productsPrice;
            
            _targetChangeText.text = FormatCashText(targetChange);
            _currentChangeText.text = FormatCashText(currentChange);

            _currentChangeText.color = Math.Abs(targetChange - currentChange) < float.Epsilon ? _correctColor : _wrongColor;
        }

        private string FormatCashText(float cash)
        {
            int integerPart = (int)cash;
            float fractionalPart = cash - integerPart;

            return $"{integerPart}.<size=80%>{fractionalPart:00}";
        }
    }
}