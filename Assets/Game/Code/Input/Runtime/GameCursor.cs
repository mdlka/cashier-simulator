using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.UserInput
{
    internal class GameCursor : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private Transform _canvasTransform;
        private RectTransform _rectTransform;

        public bool Enabled => _icon.enabled;
        public Vector2 Position =>  Enabled ? _rectTransform.position : Vector2.zero;

        private void Awake()
        {
            _canvasTransform = _icon.GetComponentInParent<Canvas>().transform;
            _rectTransform = (RectTransform)transform;
        }
        
        internal void Enable()
        {
            if (Enabled == false)
                _rectTransform.anchoredPosition = Vector3.zero;
            
            _icon.enabled = true;
        }
        
        internal void Disable()
        {
            _icon.enabled = false;
        }

        internal void Move(Vector2 delta)
        {
            if (Enabled == false)
                return;
            
            _rectTransform.anchoredPosition = ClampToScreen(_rectTransform.anchoredPosition + delta);
        }

        private Vector2 ClampToScreen(Vector2 position)
        {
            float halfWidth = Screen.width / 2f / _canvasTransform.localScale.x;
            float halfHeight = Screen.height / 2f / _canvasTransform.localScale.y;
            
            return new Vector2(
                Mathf.Clamp(position.x, -halfWidth, halfWidth), 
                Mathf.Clamp(position.y, -halfHeight, halfHeight));
        }
    }
}
