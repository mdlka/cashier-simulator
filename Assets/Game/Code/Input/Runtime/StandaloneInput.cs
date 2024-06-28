using UnityEngine;

namespace YellowSquad.CashierSimulator.UserInput
{
    public class StandaloneInput : IInput
    {
        private const int LeftMouseButton = 0;
        private const int RightMouseButton = 1;

        public bool PointerDown => Input.GetMouseButtonDown(LeftMouseButton);
        public bool PointerUp => Input.GetMouseButtonUp(LeftMouseButton);
        
        public bool Use => Input.GetMouseButtonDown(LeftMouseButton);
        public bool Undo => Input.GetMouseButtonDown(RightMouseButton);
        public bool Apply => Input.GetKeyDown(KeyCode.Space);
        
        public Vector2 AimDelta => new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        public Vector2 ScrollDelta => Input.mouseScrollDelta;
    }
}