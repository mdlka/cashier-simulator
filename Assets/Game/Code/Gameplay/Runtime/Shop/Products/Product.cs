using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Product : MonoBehaviour
    {
        [field: SerializeField] public string NameTag { get; private set; }
        [field: SerializeField] public long PriceInCents { get; private set; }
    }
}