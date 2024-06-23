using UnityEngine;

namespace gishadev.golf.Utilities
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BGMovementAnimator : MonoBehaviour
    {
        [SerializeField] private Vector2 direction;
        [SerializeField] private float speed;
        
        private MeshRenderer _meshRenderer;
        
        private void Awake() => _meshRenderer = GetComponent<MeshRenderer>();
        
        private void Update()
        {
            var offset = Time.time * speed;
            _meshRenderer.material.mainTextureOffset =  direction.normalized * offset;
        }
    }
}