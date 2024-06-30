using Cinemachine;
using DG.Tweening;
using gishadev.golf.Core;
using UnityEngine;

namespace gishadev.golf.Gameplay
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float zoomOutSize = 8f;
        [SerializeField] private float zoomTime = 0.3f;

        private CinemachineVirtualCamera _virtualCamera;
        private float _startOrthographicSize;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _startOrthographicSize = _virtualCamera.m_Lens.OrthographicSize;
        }

        private void OnEnable()
        {
            GolfClubController.ClubUp += OnClubUp;
            GolfClubController.ClubDown += OnClubDown;
            GameManager.PlayerSwitched += OnPlayerSwitched;
        }

        private void OnDisable()
        {
            GolfClubController.ClubUp -= OnClubUp;
            GolfClubController.ClubDown -= OnClubDown;
            GameManager.PlayerSwitched -= OnPlayerSwitched;
        }

        private void OnClubUp()
        {
            DOTween.To(() => _virtualCamera.m_Lens.OrthographicSize, x => _virtualCamera.m_Lens.OrthographicSize = x,
                _startOrthographicSize, zoomTime);
        }

        private void OnClubDown()
        {
            DOTween.To(() => _virtualCamera.m_Lens.OrthographicSize, x => _virtualCamera.m_Lens.OrthographicSize = x,
                zoomOutSize, zoomTime);
        }

        private void OnPlayerSwitched(GolfPlayer golfPlayer)
        {
            if (golfPlayer.GolfPlayerContainer.VirtualCamera != _virtualCamera)
            {
                _virtualCamera.Priority = 0;
                return;
            }

            _virtualCamera.Priority = 10;
        }
    }
}