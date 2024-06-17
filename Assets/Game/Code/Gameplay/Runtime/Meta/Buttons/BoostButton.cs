using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class BoostButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _activeColor;

        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Render(IReadOnlyBoost boost)
        {
            _button.gameObject.SetActive(!boost.Active);
            _background.color = boost.Active ? _activeColor : _defaultColor;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }
    }
}