using UnityEngine;

namespace gishadev.golf.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GolfBall : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }
}