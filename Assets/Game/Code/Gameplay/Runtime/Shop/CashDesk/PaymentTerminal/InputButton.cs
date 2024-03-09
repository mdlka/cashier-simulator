using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class InputButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public bool Pressed { get; private set; }

        public void Release()
        {
            Pressed = false;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Pressed = true;
        }
    }
}