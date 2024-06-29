using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class GolfField : MonoBehaviour
    {
        public BallSpawnpoint BallSpawnpoint { get; private set; }
        public GolfHole GolfHole { get; private set; }
        
        private void Awake()
        {
            BallSpawnpoint = GetComponentInChildren<BallSpawnpoint>();
            GolfHole = GetComponentInChildren<GolfHole>();
        }
    }
}