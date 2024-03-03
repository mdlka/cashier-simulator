using cakeslice;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class OutlinedObject : MonoBehaviour
    {
        [SerializeField] private Outline _outline;

        private void Awake()
        {
            _outline.enabled = false;
        }

        private void OnMouseEnter()
        {
            _outline.enabled = true;
        }

        private void OnMouseExit()
        {
            _outline.enabled = false;
        }
    }
}
