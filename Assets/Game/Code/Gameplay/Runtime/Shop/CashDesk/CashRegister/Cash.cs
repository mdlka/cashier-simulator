using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Cash : MonoBehaviour
    {
        [SerializeField] private long _totalCents;
        [SerializeField] private OutlinedObject _outlinedObject;
        [Header("Material settings")] 
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector2 _tilling;
        [SerializeField] private Vector2 _offset;

        public long TotalCents => _totalCents;

        private void Awake()
        {
            var props = new MaterialPropertyBlock();
            props.SetVector("_MainTex_ST", new Vector4(_tilling.x, _tilling.y, _offset.x, _offset.y));
            
            _meshRenderer.SetPropertyBlock(props);
        }

        public void DisableOutline()
        {
            _outlinedObject.Disable();
        }
    }
}