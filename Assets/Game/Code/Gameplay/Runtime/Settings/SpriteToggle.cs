using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class SpriteToggle : MonoBehaviour
    {
        [SerializeField] private bool _defaultState;
        [SerializeField] private Sprite _isOnIcon;
        [SerializeField] private Sprite _isOffIcon;
        [SerializeField] private Image _targetImage;
        [SerializeField] private Button _button;

        public bool CurrentState { get; private set; }

        public event Action<bool> StateChanged;

        private void OnValidate()
        {
            if (_targetImage != null && _isOnIcon != null && _isOffIcon != null)
                ChangeState(_defaultState);
        }

        private void Awake()
        {
            ChangeState(_defaultState);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void ChangeState(bool value)
        {
            CurrentState = value;
            _targetImage.sprite = value ? _isOnIcon : _isOffIcon;
            
            StateChanged?.Invoke(value);
        }

        private void OnButtonClick()
        {
            ChangeState(!CurrentState);
        }
    }
}
