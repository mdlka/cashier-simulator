using System;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class CustomerModel : MonoBehaviour
    {
        [SerializeField] private Gender _gender;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _cashContainer;
        [SerializeField] private Transform _cardContainer;
        
        public Gender Gender => _gender;
        public Animator Animator => _animator;

        public void SetupPaymentObject(PaymentObject paymentObject)
        {
            paymentObject.transform.SetParent(ContainerFor(paymentObject.PaymentMethod));
            paymentObject.transform.localPosition = Vector3.zero;
            paymentObject.transform.localRotation = Quaternion.identity;
        }

        private Transform ContainerFor(PaymentMethod paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethod.Cash => _cashContainer,
                PaymentMethod.Card => _cardContainer,
                _ => throw new ArgumentException()
            };
        }
    }
}