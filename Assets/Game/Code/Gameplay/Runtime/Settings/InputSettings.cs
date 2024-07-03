using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create InputSettings", fileName = "InputSettings", order = 56)]
    public class InputSettings : ScriptableObject
    {
        [field: NonSerialized] public float MouseSensitivity { get; private set; }
        [field: NonSerialized] public float RotationSensitivity { get; private set; }
        
        [field: SerializeField, Min(0.0001f)] internal float MinMouseSensitivity { get; private set; } = 0.5f;
        [field: SerializeField, Min(0.0001f)] internal float MaxMouseSensitivity { get; private set; } = 3f;
        [field: SerializeField, Min(0.0001f)] internal float DefaultMouseSensitivity { get; private set; } = 1f;
        [field: SerializeField, Min(0.0001f)] internal float MinRotationSensitivity { get; private set; } = 0.5f;
        [field: SerializeField, Min(0.0001f)] internal float MaxRotationSensitivity { get; private set; } = 2f;
        [field: SerializeField, Min(0.0001f)] internal float DefaultRotationSensitivity { get; private set; } = 1f;

        private void Awake()
        {
            ChangeMouseSensitivity(DefaultMouseSensitivity);
            ChangeRotationSensitivity(DefaultRotationSensitivity);
        }

        internal void ChangeMouseSensitivity(float value)
        {
            if (MinMouseSensitivity > value || value > MaxMouseSensitivity)
                throw new ArgumentOutOfRangeException(nameof(value));

            MouseSensitivity = value;
        }
        
        internal void ChangeRotationSensitivity(float value)
        {
            if (MinRotationSensitivity > value || value > MaxRotationSensitivity)
                throw new ArgumentOutOfRangeException(nameof(value));

            RotationSensitivity = value;
        }
    }
}