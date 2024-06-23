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
        [Inject] private IGameManager _gameManager;

        public static event Action ClubDown;
        public static event Action ClubPunch;
        public static event Action ClubUp;
        
        private GolfBall SelectedGolfBall => _gameManager.CurrentTurnPlayer.GolfPlayerContainer.GolfBall;

        private Vector2 _punchDirection;
        private float _punchForcePercentage;

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

        private void Update()
        {
            if (SelectedGolfBall.Velocity.magnitude > 0) return;

            if (_isClubDown)
            {
                _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

                var punchVector = (Vector2) SelectedGolfBall.transform.position - _mousePos;
                _punchDirection = punchVector.normalized;
                _punchForcePercentage = Mathf.Min(punchVector.magnitude, _gameDataSO.MaxLineLength) /
                                       _gameDataSO.MaxLineLength;

                _lr.SetPosition(0, SelectedGolfBall.transform.position);
                _lr.SetPosition(1,
                    (Vector2) SelectedGolfBall.transform.position -
                    _punchDirection * (_punchForcePercentage * _gameDataSO.MaxLineLength));
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
            if (SelectedGolfBall.Velocity.magnitude > 0) return;
            
            _lr.enabled = false;
            _isClubDown = false;
            SelectedGolfBall.AddImpulseForce(_punchDirection * (_punchForcePercentage * _gameDataSO.MaxPunchForce));
            
            ClubUp?.Invoke();
            ClubPunch?.Invoke();
        }

        private void OnClubDown(InputAction.CallbackContext obj)
        {
            if (SelectedGolfBall.Velocity.magnitude > 0) return;
            
            _lr.enabled = true;
            _isClubDown = true;
            
            ClubDown?.Invoke();
        }
    }
}