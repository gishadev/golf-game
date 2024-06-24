using UnityEngine;

namespace gishadev.golf.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [SerializeField] private float maxLineLength = 3.5f;
        [SerializeField] private float maxPunchForce = 13f;

        [SerializeField] private GameObject holePrefab;
        [SerializeField] private Material fieldMaterial;
        [SerializeField] private Material fieldLineMaterial;
        
        
        public float MaxLineLength => maxLineLength;
        public float MaxPunchForce => maxPunchForce;
        public Material FieldMaterial => fieldMaterial;
        public Material FieldLineMaterial => fieldLineMaterial;
        public GameObject HolePrefab => holePrefab;
    }
}