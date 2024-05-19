using QuickOutline;
using UnityEngine;

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
            _outline.enabled = _needOutline;
        }

        private void OnMouseExit()
        {
            _outline.enabled = false;
        }
    }
}
