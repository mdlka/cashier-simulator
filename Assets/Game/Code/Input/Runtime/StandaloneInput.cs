﻿using UnityEngine;

namespace YellowSquad.CashierSimulator.UserInput
{
    public class StandaloneInput : IInput
    {
        private const int LeftMouseButton = 0;
        
        public bool Use => Input.GetMouseButtonDown(LeftMouseButton);
        public bool Apply => Input.GetKeyDown(KeyCode.Space);
        public Vector2 AimDelta => new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}