using UnityEngine;

namespace YellowSquad.CashierSimulator.UserInput
{
    public interface IInput
    {
        bool Use { get; }
        
        Vector2 AimDelta { get; }
    }
}
