using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Product : MonoBehaviour
    {
        [field: SerializeField] public long PriceInCents { get; private set; }
    }
}