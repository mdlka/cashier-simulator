using QuickOutline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class OutlinedObject : MonoBehaviour
    {
        [SerializeField] private Outline _outline;

        private bool _needOutline = true;

        private void OnEnable()
        {
            _outline.enabled = false;
        }

        public void Disable()
        {
            _needOutline = false;
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            _outline.enabled = _needOutline;
        }
        
        private void OnMouseOver()
        {
            if (_outline.enabled && EventSystem.current.IsPointerOverGameObject())
                _outline.enabled = false;
        }

        private void OnMouseExit()
        {
            _outline.enabled = false;
        }
    }
}
