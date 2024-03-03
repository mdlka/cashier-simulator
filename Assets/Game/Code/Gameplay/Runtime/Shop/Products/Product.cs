using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Product : MonoBehaviour
    {
        [field: SerializeField] public float PriceInDollars { get; private set; }
    }
}