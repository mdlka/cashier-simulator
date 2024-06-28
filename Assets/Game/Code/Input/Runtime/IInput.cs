using UnityEngine;

namespace YellowSquad.CashierSimulator.UserInput
{
    public interface IInput
    {
        bool Use { get; }
        bool Undo { get; }
        bool Apply { get; }
        
        Vector2 AimDelta { get; }
    }
}
