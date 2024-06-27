using System.Collections.Generic;
using System.Linq;
using gishadev.golf.Core;
using mattatz.Triangulation2DSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace gishadev.golf.Utilities
{
    [RequireComponent(typeof(SplineContainer))]
    [ExecuteInEditMode]
    public class ShapesGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject shapeObject;
        [Space] [SerializeField] private Material mainMaterial;
        [SerializeField] private Material edgesMaterial;
        [SerializeField] private bool isSolid;
        [Space] [SerializeField] private string assetToSaveName = "Shape";

        [Inject] private GameDataSO gameDataSO;

        private SplineContainer _splineContainer;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private LineRenderer _lineRenderer;
        private Mesh _generatedMesh;

        private EdgeCollider2D _edgeCollider;
        private PolygonCollider2D _polygonCollider;

        private readonly string _prefabFolderPath = "Assets/_Project/Prefabs/Fields";
        private readonly string _meshFolderPath = "Assets/_Project/GeneratedMeshes";

#if UNITY_EDITOR
        private void Start() => Initialize();
        private void LateUpdate() => GenerateShape();
#endif

        [Button(ButtonSizes.Large)]
        private void Initialize()
        {
            _splineContainer = GetComponent<SplineContainer>();

            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(i).gameObject);
            shapeObject = new GameObject("Shape");
            shapeObject.transform.SetParent(transform);

            _meshFilter = shapeObject.AddComponent<MeshFilter>();
            _meshRenderer = shapeObject.AddComponent<MeshRenderer>();

            _meshRenderer.material = mainMaterial;
            shapeObject.transform.localPosition = Vector3.zero;

            if (!isSolid)
            {
                _lineRenderer = shapeObject.AddComponent<LineRenderer>();
                _edgeCollider = shapeObject.AddComponent<EdgeCollider2D>();
                InitializeLines();
            }
            else
                _polygonCollider = shapeObject.AddComponent<PolygonCollider2D>();
        }

        [Button, HorizontalGroup("AddButtons")]
        private void AddHole()
        {
            PrefabUtility.InstantiatePrefab(gameDataSO.HolePrefab, shapeObject.transform);
        }

        [Button, HorizontalGroup("AddButtons")]
        private void AddSpawnpoint()
        {
        }

        [Button, HorizontalGroup("SaveButtons")]
        private void SaveMesh()
        {
            // Saving mesh.
            string meshPath = $"{_meshFolderPath}/{assetToSaveName}.asset";
            if (!string.IsNullOrEmpty(meshPath))
            {
                AssetDatabase.CreateAsset(_generatedMesh, meshPath);
                AssetDatabase.SaveAssets();
            }

            _meshFilter.mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        }

        [Button, HorizontalGroup("SaveButtons")]
        private void SavePrefab()
        {
            // Saving prefab.
            string prefabPath = $"{_prefabFolderPath}/{assetToSaveName}.prefab";
            if (shapeObject != null && !string.IsNullOrEmpty(prefabPath))
                PrefabUtility.SaveAsPrefabAsset(shapeObject, prefabPath);
        }

        private void GenerateShape()
        {
            if (_splineContainer.Splines.Count == 0)
                return;
            
            var knots = _splineContainer.Splines[0].Knots.ToArray();
            if (knots.Length == 0)
                return;
            
            GenerateMesh(knots);
            if (!isSolid)
                SetEdge(knots);
            else
                SetPolygon(knots);
        }
        
        private void SetEdge(BezierKnot[] knots)
        {
            _lineRenderer.positionCount = knots.Length;

            var positions = knots
                .Select(x => (Vector3) x.Position)
                .ToArray();
            _lineRenderer.SetPositions(positions);

            // loop edge collider, add first point to the end.
            var edgeColliderPoints = positions
                .Select(x => (Vector2) x)
                .ToList();
            edgeColliderPoints.Add(edgeColliderPoints[0]);
            _edgeCollider.points = edgeColliderPoints.ToArray();
        }

        private void SetPolygon(BezierKnot[] knots)
        {
            var positions = knots
                .Select(x => (Vector3) x.Position)
                .ToArray();

            var polygonColliderPoints = positions
                .Select(x => (Vector2) x)
                .ToList();
            polygonColliderPoints.Add(polygonColliderPoints[0]);
            _polygonCollider.points = polygonColliderPoints.ToArray();
        }

        private void GenerateMesh(BezierKnot[] knots)
        {
            var vertices = knots
                .Select(x => (Vector3) x.Position)
                .ToArray();

            List<Vector2> points = new List<Vector2>();
            points.AddRange(vertices.Select(x => (Vector2) x));

            Polygon2D polygon = Polygon2D.Contour(points.ToArray());
            Triangulation2D triangulation = new Triangulation2D(polygon, 22.5f);

            _generatedMesh = triangulation.Build();
            _meshFilter.mesh = _generatedMesh;
        }


        private void InitializeLines()
        {
            _lineRenderer.loop = true;
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.startWidth = 0.5f;
            _lineRenderer.endWidth = 0.5f;
            _lineRenderer.material = edgesMaterial;

            _edgeCollider.edgeRadius = 0.25f;
        }
    }
}