using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ProductTape : MonoBehaviour
    {
        [SerializeField] private Transform[] _places;
        
        public bool HasProducts { get; private set; }

        public void Add(Product product)
        {
            
        }
    }
}