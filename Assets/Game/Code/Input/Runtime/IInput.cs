using UnityEngine;

namespace YellowSquad.CashierSimulator.UserInput
{
    public interface IInput
    {
        bool PointerDown { get; }
        bool PointerUp { get; }
        
        bool Use { get; }
        bool Undo { get; }
        bool Apply { get; }
        
        Vector2 AimDelta { get; }
        Vector2 ScrollDelta { get; }
    }
}
