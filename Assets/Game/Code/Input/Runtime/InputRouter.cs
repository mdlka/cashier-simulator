﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using YellowSquad.CashierSimulator.Gameplay;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.UserInput
{
    public class InputRouter : MonoBehaviour
    {
        private readonly List<RaycastResult> _raycastResults = new();

        [SerializeField] private GameCursor _cursor;
        [SerializeField] private CameraAim _cameraAim;
        [SerializeField] private CashDesk _cashDesk;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField, Min(0.001f)] private float _baseMouseSensitivity = 21.67f;

        private IInput _input;
        private Camera _camera;
        private InputSettings _inputSettings;

        private void Awake()
        {
            _camera = Camera.main;
            _input = new StandaloneInput();
            _inputSettings = _gameSettings.InputSettings;
        }

        private void Update()
        {
            _cursor.Move(_input.AimDelta * _baseMouseSensitivity * _inputSettings.MouseSensitivity);
        }

        public void UpdateInput()
        {
            if (_gameSettings.Opened)
                return;

            if (_input.OpenSettings)
            {
                _gameSettings.Open();
                SetActiveCursor(true);
                return;
            }
            
            SetActiveGameCursor(_cashDesk.PaymentTerminal.Active);

            if (_cashDesk.PaymentTerminal.Active == false)
                _cameraAim.RotateAim(_input.AimDelta * _inputSettings.RotationSensitivity);

            if (_input.Apply)
                _cashDesk.CashRegister.EndPayment();

            if (_input.Use == false && _input.Undo == false)
                return;

            var pointerPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
            
            if (IsPointerOverUIObject(pointerPosition))
                return;

            if (Physics.Raycast(_camera.ScreenPointToRay(pointerPosition), out RaycastHit hitInfo) == false)
                return;

            if (_input.Use && hitInfo.transform.root.TryGetComponent(out Product product))
            {
                _cashDesk.ProductScanner.Scan(product);
            }
            else if (hitInfo.transform.TryGetComponentInParent(out CashSlot cashSlot))
            {
                if (_input.Use)
                    _cashDesk.CashRegister.Take(cashSlot);
                else if (_input.Undo)
                    _cashDesk.CashRegister.Return(cashSlot);
            }
            else if (_input.Use && hitInfo.transform.TryGetComponentInParent(out PaymentObject paymentObject))
            {
                _cashDesk.AcceptPaymentObject(paymentObject);
            }
        }

        public void SetActiveCursor(bool value)
        {
            if (value)
                SetActiveGameCursor(false);
            
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = value;
        }

        public void ResetCameraRotation()
        {
            _cameraAim.ResetRotation();
        }

        private void SetActiveGameCursor(bool value)
        {
            if (value == _cursor.Enabled)
                return;
            
            if (value)
                _cursor.Enable();
            else
                _cursor.Disable();
        }

        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = inputPosition };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Any(result => result.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
    }
}