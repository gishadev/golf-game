using System;
using gisha.golf;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private CustomInput _input;
        private bool _isClubDown;
        
        private void Awake()
        {
            _cam = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            _lr = GetComponentInChildren<LineRenderer>();
            _input = new CustomInput();
        }

        private void Start()
        {
            ChangeBodyType(RigidbodyType2D.Dynamic);
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Gameplay.ClubDown.performed += OnClubDown;
            _input.Gameplay.ClubUp.performed += OnClubUp;
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Gameplay.ClubDown.performed -= OnClubDown;
            _input.Gameplay.ClubUp.performed -= OnClubUp;
        }
        
        private void Update()
        {
            if (_isClubDown)
            {
                _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

                var punchVector = (Vector2) transform.position - _mousePos;
                PunchDirection = punchVector.normalized;
                PunchForcePercentage = Mathf.Min(punchVector.magnitude, maxLineLength) / maxLineLength;

                _lr.SetPosition(0, transform.position);
                _lr.SetPosition(1,
                    (Vector2) transform.position - PunchDirection * (PunchForcePercentage * maxLineLength));
            }
        }

        public void ChangeBodyType(RigidbodyType2D bodyType)
        {
            _rb.bodyType = bodyType;
        }
        
        private void OnClubUp(InputAction.CallbackContext obj)
        {
            _lr.enabled = false;
            _isClubDown = false;
            _rb.AddForce(PunchDirection * (PunchForcePercentage * maxPunchForce), ForceMode2D.Impulse);
        }

        private void OnClubDown(InputAction.CallbackContext obj)
        {
            _lr.enabled = true;
            _isClubDown = true;
        }


    }
}