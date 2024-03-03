using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CameraAim : MonoBehaviour
    {
        [SerializeField, Range(0f, 90f)] private float _upperAngleLimit = 70f;
        [SerializeField, Range(0f, 90f)] private float _lowerAngleLimit = 80f;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void RotateAim(Vector2 deltaAngle)
        {
            float currentEulerY = Mathf.DeltaAngle(0f, _camera.transform.eulerAngles.y);
            float currentEulerX = Mathf.DeltaAngle(0f, Vector3.Angle(_camera.transform.forward, Vector3.up) - 90f);

            var newEulerY = currentEulerY + deltaAngle.x;
            var newEulerX = Mathf.Clamp(currentEulerX - deltaAngle.y, -_upperAngleLimit, _lowerAngleLimit);
            
            _camera.transform.rotation = Quaternion.Euler(newEulerX, newEulerY, 0f);
        }
    }
}