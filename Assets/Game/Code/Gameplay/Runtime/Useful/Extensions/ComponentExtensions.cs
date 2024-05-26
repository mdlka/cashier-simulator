using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay.Useful
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInParent<T>(this Component component, out T value) where T : Component
        {
            value = component.GetComponentInParent<T>();
            return value != null;
        }
    }
}