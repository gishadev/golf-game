using UnityEngine;

namespace gishadev.golf.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [SerializeField] private float maxLineLength = 3.5f;
        [SerializeField] private float maxPunchForce = 13f;

        [SerializeField] private GameObject holePrefab;
        [SerializeField] private GameObject ballSpawnpointPrefab;
        
        public float MaxLineLength => maxLineLength;
        public float MaxPunchForce => maxPunchForce;
        public GameObject HolePrefab => holePrefab;

        public GameObject BallSpawnpointPrefab => ballSpawnpointPrefab;
    }
}