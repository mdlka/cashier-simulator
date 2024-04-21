using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class CashStack : MonoBehaviour
    {
        private readonly List<Cash> _cash = new();
        
        [SerializeField, Min(0)] private float _offsetY;
        [SerializeField] private Transform _content;

        public void Add(Cash cash)
        {
            cash.transform.DOJump(_content.position + Vector3.up * _offsetY * _cash.Count, 0.1f, 1, 0.5f);
            cash.transform.DORotate(_content.rotation.eulerAngles, 0.5f);
            
            _cash.Add(cash);
        }

        public void Remove(Cash cash)
        {
            if (_cash.Contains(cash) == false)
                throw new InvalidOperationException();

            int index = _cash.IndexOf(cash);
            _cash.Remove(cash);

            for (int i = index; i < _cash.Count; i++)
                _cash[i].transform.DOMove(_content.position + Vector3.up * _offsetY * (i+1), 0.01f);
        }
    }
}