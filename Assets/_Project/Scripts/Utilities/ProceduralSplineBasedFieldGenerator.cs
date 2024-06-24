using System.Collections.Generic;
using System.Linq;
using gishadev.golf.Core;
using mattatz.Triangulation2DSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

namespace gishadev.golf.Utilities
{
    [RequireComponent(typeof(SplineContainer))]
    [ExecuteInEditMode]
    public class ProceduralSplineBasedFieldGenerator : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameDataSO;

        private SplineContainer _splineContainer;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private GameObject _fieldObject;

        private void Start() => Initialize();
        private void LateUpdate() => GenerateField();

        [Button]
        private void Initialize()
        {
            _splineContainer = GetComponent<SplineContainer>();

            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(i).gameObject);
            _fieldObject = new GameObject("Field");
            _fieldObject.transform.SetParent(transform);

            _meshFilter = _fieldObject.AddComponent<MeshFilter>();
            _meshRenderer = _fieldObject.AddComponent<MeshRenderer>();
            _edgeCollider = _fieldObject.AddComponent<EdgeCollider2D>();
            _lineRenderer = _fieldObject.AddComponent<LineRenderer>();
                
            _meshRenderer.material = gameDataSO.FieldMaterial;
            _fieldObject.transform.localPosition = Vector3.zero;

            InitializeLines();
        }

        private void GenerateField()
        {
            var knots = _splineContainer.Splines[0].Knots.ToArray();
            GenerateMesh(knots);
            GenerateEdge(knots);
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

            Mesh mesh = triangulation.Build();
            _meshFilter.mesh = mesh;
        }

        private void GenerateEdge(BezierKnot[] knots)
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

        private void InitializeLines()
        {
            _lineRenderer.loop = true;
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.startWidth = 0.5f;
            _lineRenderer.endWidth = 0.5f;
            _lineRenderer.material = gameDataSO.FieldLineMaterial;
            
            _edgeCollider.edgeRadius = 0.25f;
        }
    }
}