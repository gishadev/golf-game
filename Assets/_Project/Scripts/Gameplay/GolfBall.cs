using UnityEngine;

namespace gishadev.golf.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GolfBall : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();
        private void Start() => ChangeBodyType(RigidbodyType2D.Dynamic);
        public void ChangeBodyType(RigidbodyType2D bodyType) => _rb.bodyType = bodyType;
        public void AddImpulseForce(Vector2 force) => _rb.AddForce(force, ForceMode2D.Impulse);
    }
}