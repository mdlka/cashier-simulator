using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashDeskHelpBox : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Pair[] _helpsObjects;

        private Pair _currentHelpObject;
        
        public enum State
        {
            Idle,
            Scan,
            AcceptPayment,
            CashRegister,
            PaymentTerminal
        }

        private void Awake()
        {
            foreach (var helpObject in _helpsObjects)
                foreach (var targetObject in helpObject.TargetObjects)
                    targetObject.SetActive(false);

            _backgroundImage.enabled = false;
        }

        public void Switch(State state)
        {
            if (_currentHelpObject != null)
                foreach (var targetObject in _currentHelpObject.TargetObjects)
                    targetObject.SetActive(false);

            _currentHelpObject = _helpsObjects.First(helpObject => helpObject.State == state);
            _backgroundImage.enabled = _currentHelpObject.TargetObjects.Length != 0;
            
            foreach (var targetObject in _currentHelpObject.TargetObjects)
                targetObject.SetActive(true);
        }

        [Serializable]
        private class Pair
        {
            [field: SerializeField] public State State { get; private set; }
            [field: SerializeField] public GameObject[] TargetObjects { get; private set; }
        }
    }
}
