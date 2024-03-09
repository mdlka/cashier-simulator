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
        
        [SerializeField] private CameraAim _cameraAim;
        [SerializeField] private ProductScanner _productScanner;
        [SerializeField] private CashRegister _cashRegister;
        [SerializeField, Min(0.001f)] private float _sensitivity;

        private IInput _input;
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            _input = new StandaloneInput();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            _cameraAim.RotateAim(_input.AimDelta * _sensitivity);
            
            if (_input.Apply)
                _cashRegister.EndPayment();

            if (_input.Use == false)
                return;

            var pointerPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
            
            if (IsPointerOverUIObject(pointerPosition))
                return;
            
            var ray = _camera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo) == false)
                return;
            
            if (hitInfo.transform.root.TryGetComponent(out Product product))
                _productScanner.Scan(product);
            else if (hitInfo.transform.TryGetComponentInParent(out CashSlot cashSlot))
                _cashRegister.Take(cashSlot);
        }
        
        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = inputPosition };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Any(result => result.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
    }
}