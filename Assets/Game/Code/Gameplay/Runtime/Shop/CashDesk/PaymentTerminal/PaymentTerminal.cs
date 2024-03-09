using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class PaymentTerminal : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private TMP_Text _screenText;
        [SerializeField] private InputButton _okButton;
        [SerializeField] private PaymentTerminalButton[] _buttons;

        private string _currentPrice = "";
        private CultureInfo _cultureInfo;

        public Transform CameraPoint => _cameraPoint;
        public bool Active { get; private set; }

        private void Awake()
        {
            _cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            _cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        }

        public IEnumerator AcceptPayment(float targetPrice)
        {
            Active = true;
            ReleaseButtons();
            
            while (_okButton.Pressed == false)
            {
                yield return new WaitUntil(() => _buttons.Any(button => button.Pressed) || _okButton.Pressed);

                if (_okButton.Pressed)
                {
                    if (ValidateInputPrice(targetPrice))
                        break;
                    
                    Debug.Log($"No. Need {targetPrice}, but was {_screenText.text}");
                    _okButton.Release();
                    continue;
                }

                var pressedButton = _buttons.First(button => button.Pressed);
                pressedButton.Release();
                
                ApplyInput(pressedButton.Type);
            }

            _currentPrice = "";
            UpdateScreenText();

            Active = false;
        }

        private void ReleaseButtons()
        {
            foreach (var button in _buttons)
                button.Release();
            
            _okButton.Release();
        }

        private bool ValidateInputPrice(float targetPrice)
        {
            if (float.TryParse(_screenText.text, NumberStyles.Any, _cultureInfo, out float value) == false)
                return false;
            
            return Math.Abs(value - targetPrice) < float.Epsilon;
        }

        private void ApplyInput(PaymentTerminalButtonType type)
        {
            if (type == PaymentTerminalButtonType.Delete)
            {
                if (_currentPrice.Length == 0)
                    return;
                
                _currentPrice = _currentPrice[..^1];
            }
            else if (type == PaymentTerminalButtonType.Dot)
            {
                if (_currentPrice.Contains('.'))
                    return;

                if (_currentPrice.Length == 0)
                    _currentPrice += '0';
                
                _currentPrice += '.';
            }
            else if (_currentPrice.Length < 6)
            {
                _currentPrice += (char)(48 + type);
            }
            
            UpdateScreenText();
        }

        private void UpdateScreenText()
        {
            _screenText.text = _currentPrice.Length > 0 ? _currentPrice : "00.00";
        }
    }
}
