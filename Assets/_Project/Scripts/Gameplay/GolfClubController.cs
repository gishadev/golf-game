using System;
using gisha.golf;
using gishadev.golf.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace gishadev.golf.Gameplay
{
    public class GolfClubController : MonoBehaviour
    {
        [Inject] private GameDataSO _gameDataSO;

        public GolfBall SelectedGolfBall { get; private set; }

        private Vector2 PunchDirection { get; set; }
        private float PunchForcePercentage { get; set; }

        private CustomInput _input;
        private LineRenderer _lr;
        private bool _isClubDown;
        private Camera _cam;
        private Vector2 _mousePos;

        public void Awake()
        {
            _cam = Camera.main;

            _lr = GetComponentInChildren<LineRenderer>();
            _input = new CustomInput();
        }

        private void Start()
        {
            SelectedGolfBall = FindObjectOfType<GolfBall>();
        }

        private void Update()
        {
            if (_isClubDown)
            {
                _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

                var punchVector = (Vector2) SelectedGolfBall.transform.position - _mousePos;
                PunchDirection = punchVector.normalized;
                PunchForcePercentage = Mathf.Min(punchVector.magnitude, _gameDataSO.MaxLineLength) /
                                       _gameDataSO.MaxLineLength;

                _lr.SetPosition(0, SelectedGolfBall.transform.position);
                _lr.SetPosition(1,
                    (Vector2) SelectedGolfBall.transform.position - PunchDirection * (PunchForcePercentage * _gameDataSO.MaxLineLength));
            }
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Gameplay.ClubDown.performed += OnClubDown;
            _input.Gameplay.ClubUp.performed += OnClubUp;
        }

        private void OnDisable()
        {
            _input.Gameplay.ClubDown.performed -= OnClubDown;
            _input.Gameplay.ClubUp.performed -= OnClubUp;
            _input.Disable();
        }

        private void OnClubUp(InputAction.CallbackContext obj)
        {
            _lr.enabled = false;
            _isClubDown = false;
            SelectedGolfBall.AddImpulseForce(PunchDirection * (PunchForcePercentage * _gameDataSO.MaxPunchForce));
        }

        private void OnClubDown(InputAction.CallbackContext obj)
        {
            _lr.enabled = true;
            _isClubDown = true;
        }
    }
}