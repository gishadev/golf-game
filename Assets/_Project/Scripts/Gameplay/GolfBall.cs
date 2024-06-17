using UnityEngine;

namespace gishadev.golf.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GolfBall : MonoBehaviour
    {
        public Vector2 Velocity => _rb.velocity;

        private Rigidbody2D _rb;
        private void Awake() => _rb = GetComponent<Rigidbody2D>();
        private void Start() => ChangeBodyType(RigidbodyType2D.Dynamic);

        private void FixedUpdate()
        {
            if (Velocity.magnitude is > 0f and < 0.1f)
                _rb.velocity = Vector2.zero;
        }

        public void ChangeBodyType(RigidbodyType2D bodyType) => _rb.bodyType = bodyType;
        public void AddImpulseForce(Vector2 force) => _rb.AddForce(force, ForceMode2D.Impulse);
    }
}