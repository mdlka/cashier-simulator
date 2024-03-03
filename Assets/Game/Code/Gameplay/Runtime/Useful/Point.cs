using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay.Useful
{
    public class Point<T> : MonoBehaviour where T : class
    {
        public Vector3 Position => transform.position;
        public bool IsBusy => Value != null;
        public T Value { get; private set; }

        public void Take(T value)
        {
            if (IsBusy)
                throw new InvalidOperationException();
            
            Value = value;
        }

        public void Free()
        {
            if (IsBusy == false)
                throw new InvalidOperationException();
            
            Value = null;
        }
    }
}