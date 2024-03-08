using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Cash : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float Value { get; private set; }
    }
}