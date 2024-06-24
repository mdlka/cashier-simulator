using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.UserInput
{
    internal class GameCursor : MonoBehaviour
    {
        private readonly List<RaycastResult> _raycastResults = new();
        
        [SerializeField] private Image _icon;

        private Transform _canvasTransform;
        private RectTransform _rectTransform;

        private GameObject _lastPointerDownObject;
        private PointerEventData _pointerEventData;
        private EventSystem _eventSystem;

        public bool Enabled => _icon.enabled;

        private void Awake()
        {
            _canvasTransform = _icon.GetComponentInParent<Canvas>().transform;
            _rectTransform = (RectTransform)_icon.transform;
            
            _eventSystem = EventSystem.current;
            _pointerEventData = new PointerEventData(_eventSystem);
        }

        internal void Move(Vector2 delta)
        {
            _rectTransform.anchoredPosition = ClampToScreen(_rectTransform.anchoredPosition + delta);
        }

        internal void PointerDown()
        {
            if (Enabled == false)
                return;
            
            _raycastResults.Clear();
            _pointerEventData.position = _rectTransform.position;

            _eventSystem.RaycastAll(_pointerEventData, _raycastResults);
            
            if (_raycastResults.Count <= 0) 
                return;
            
            ExecuteEvents.Execute(_raycastResults[0].gameObject, _pointerEventData, ExecuteEvents.pointerDownHandler);
            _lastPointerDownObject = _raycastResults[0].gameObject;
        }

        internal void PointerUp()
        {
            if (Enabled == false)
                return;
            
            _raycastResults.Clear();
            _pointerEventData.position = _rectTransform.position;

            _eventSystem.RaycastAll(_pointerEventData, _raycastResults);
            
            if (_lastPointerDownObject != null) 
                ExecuteEvents.Execute(_lastPointerDownObject, _pointerEventData, ExecuteEvents.pointerUpHandler);

            if (_raycastResults.Count <= 0) 
                return;
            
            ExecuteEvents.Execute(_raycastResults[0].gameObject, _pointerEventData, ExecuteEvents.pointerUpHandler);
            
            if (_lastPointerDownObject == _raycastResults[0].gameObject)
                ExecuteEvents.Execute(_raycastResults[0].gameObject, _pointerEventData, ExecuteEvents.pointerClickHandler);

            _lastPointerDownObject = null;
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
