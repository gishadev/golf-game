using UnityEngine;

namespace gishadev.golf.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GolfBall : MonoBehaviour
    {
        [SerializeField] private float maxLineLength = 2f;
        [SerializeField] private float maxPunchForce = 15f;

        public Vector2 PunchDirection { get; private set; }
        public float PunchForcePercentage { get; private set; }

        private Vector2 _mousePos;

        private Rigidbody2D _rb;
        private Camera _cam;
        private LineRenderer _lr;

        private void Awake()
        {
            _cam = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            _lr = GetComponentInChildren<LineRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _lr.enabled = true;

            if (Input.GetMouseButtonUp(0))
            {
                _lr.enabled = false;
                _rb.AddForce(PunchDirection * (PunchForcePercentage * maxPunchForce), ForceMode2D.Impulse);
            }

            if (Input.GetMouseButton(0))
            {
                _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

                var punchVector =  (Vector2) transform.position - _mousePos;
                PunchDirection = punchVector.normalized;
                PunchForcePercentage = Mathf.Min(punchVector.magnitude, maxLineLength) / maxLineLength;

                _lr.SetPosition(0, transform.position);
                _lr.SetPosition(1,
                    (Vector2) transform.position - PunchDirection * (PunchForcePercentage * maxLineLength));
            }
        }
    }
}