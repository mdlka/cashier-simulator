using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay;

namespace YellowSquad.CashierSimulator.UserInput
{
    public class InputRouter : MonoBehaviour
    {
        [SerializeField] private CameraAim _cameraAim;
        [SerializeField, Min(0.001f)] private float _sensitivity;

        private IInput _input;
        
        private void Awake()
        {
            _input = new StandaloneInput();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            _cameraAim.RotateAim(_input.AimDelta * _sensitivity);
        }
    }
}