using Cinemachine;
using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class GolfPlayerContainer : MonoBehaviour
    {
        public CinemachineVirtualCamera VirtualCamera { get; private set; }
        public GolfBall GolfBall { get; private set; }

        private void Awake()
        {
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            GolfBall = GetComponentInChildren<GolfBall>();
        }
    }
}