using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    [CreateAssetMenu(menuName = "Cashier Simulator/Create InputSettings", fileName = "InputSettings", order = 56)]
    public class InputSettings : ScriptableObject
    {
        [Header("Default settings")] 
        [SerializeField, Min(0.0001f)] private float _defaultMouseSensitivity = 1f;
        [SerializeField, Min(0.0001f)] private float _defaultRotationSensitivity = 1f;
        
        [field: NonSerialized] public float MouseSensitivity { get; private set; }
        [field: NonSerialized] public float RotationSensitivity { get; private set; }
        
        [field: Header("Clamp settings")]
        [field: SerializeField, Min(0.0001f)] internal float MinMouseSensitivity { get; private set; } = 0.5f;
        [field: SerializeField, Min(0.0001f)] internal float MaxMouseSensitivity { get; private set; } = 3f;
        [field: SerializeField, Min(0.0001f)] internal float MinRotationSensitivity { get; private set; } = 0.5f;
        [field: SerializeField, Min(0.0001f)] internal float MaxRotationSensitivity { get; private set; } = 2f;

        internal void Initialize()
        {
            ChangeMouseSensitivity(_defaultMouseSensitivity);
            ChangeRotationSensitivity(_defaultRotationSensitivity);
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