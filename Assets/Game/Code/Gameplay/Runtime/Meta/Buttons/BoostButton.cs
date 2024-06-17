using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay.Meta
{
    internal class BoostButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

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
            _button.interactable = !boost.Active;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }
    }
}