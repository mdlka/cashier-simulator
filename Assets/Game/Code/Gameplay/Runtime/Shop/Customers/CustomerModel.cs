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
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;

        public Gender Gender => _gender;
        public Animator Animator => _animator;

        public void SetupPaymentObject(PaymentObject paymentObject)
        {
            paymentObject.transform.SetParent(ContainerFor(paymentObject.PaymentMethod));
            paymentObject.transform.localPosition = Vector3.zero;
            paymentObject.transform.localRotation = Quaternion.identity;
        }

        public void SetupMaterial(Material material)
        {
            foreach (var meshRenderer in _meshRenderers)
                meshRenderer.sharedMaterial = material;
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