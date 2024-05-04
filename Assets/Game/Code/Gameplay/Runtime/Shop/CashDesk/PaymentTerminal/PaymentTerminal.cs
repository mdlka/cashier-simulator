using System.Collections;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class PaymentTerminal : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _maxInputPriceLenght;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private Transform _cardPoint;
        [SerializeField] private TMP_Text _screenText;
        [SerializeField] private InputButton _okButton;
        [SerializeField] private PaymentTerminalButton[] _buttons;
        [Header("Error animation")]
        [SerializeField] private float _errorStepAnimationDuration;
        [SerializeField] private Image _screenBackground;
        [SerializeField] private Color _errorColor;

        private string _currentPrice = "";
        private CultureInfo _cultureInfo;
        private Color _defaultScreenColor;
        private float _epsilon;

        public Transform CameraPoint => _cameraPoint;
        public bool Active { get; private set; }

        private void Awake()
        {
            _cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            _cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            _defaultScreenColor = _screenBackground.color;
            _epsilon = Mathf.Pow(0.1f, _maxInputPriceLenght - 2);
        }

        public IEnumerator AcceptPayment(PaymentObject card, Currency targetPrice)
        {
            Active = true;
            ReleaseButtons();
            
            card.MoveTo(_cardPoint, animationDuration: 0.75f);
            card.DisableOutline();
            
            while (_okButton.Pressed == false)
            {
                yield return new WaitUntil(() => _buttons.Any(button => button.Pressed) || _okButton.Pressed);

                if (_okButton.Pressed)
                {
                    if (ValidateInputPrice(targetPrice))
                        break;

                    PlayErrorAnimation();
                    _okButton.Release();
                    
                    continue;
                }

                var pressedButton = _buttons.First(button => button.Pressed);
                pressedButton.Release();
                
                ApplyInput(pressedButton.Type);
            }

            _currentPrice = "";
            UpdateScreenText();
            
            card.Destroy();

            Active = false;
        }

        private void ReleaseButtons()
        {
            foreach (var button in _buttons)
                button.Release();
            
            _okButton.Release();
        }

        private bool ValidateInputPrice(in Currency targetPrice)
        {
            string[] splitValue = _screenText.text.Split('.');
            string cents = splitValue.Length == 2 ? !string.IsNullOrEmpty(splitValue[1]) ? splitValue[1] : "0" : "0";

            int centsMultiplier = cents.Length switch
            {
                2 when cents[0] != '0' => 1,
                _ => cents[0] == '0' ? 1 : 10
            };
            
            return int.Parse(splitValue[0]) == targetPrice.Dollars && int.Parse(cents) * centsMultiplier == targetPrice.Cents;
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

                if (_currentPrice.Length >= _maxInputPriceLenght)
                    return;

                if (_currentPrice.Length == 0)
                    _currentPrice += '0';
                
                _currentPrice += '.';
            }
            else if (_currentPrice.Length < _maxInputPriceLenght)
            {
                if (_currentPrice.Length == 1 && _currentPrice[0] == '0')
                    return;

                if (_currentPrice.Contains('.') && _currentPrice.Split('.')[1].Length == 2)
                    return;
                
                _currentPrice += (char)(48 + type);
            }
            
            UpdateScreenText();
        }

        private void PlayErrorAnimation()
        {
            transform.DOComplete(true);
            _screenBackground.color = _defaultScreenColor;
            
            var sequence = DOTween.Sequence();
            sequence.Append(_screenBackground.DOColor(_errorColor, _errorStepAnimationDuration));
            sequence.Append(_screenBackground.DOColor(_defaultScreenColor, _errorStepAnimationDuration));
        }

        private void UpdateScreenText()
        {
            _screenText.text = _currentPrice.Length > 0 ? _currentPrice : "00.00";
        }
    }
}
