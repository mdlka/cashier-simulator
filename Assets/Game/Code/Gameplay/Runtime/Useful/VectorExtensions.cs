using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay.Useful
{
    public static class VectorExtensions
    {
        public static Vector3 XZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }
    }
}