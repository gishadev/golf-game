using System.Collections.Generic;
using System.Linq;
using gishadev.golf.Core;
using mattatz.Triangulation2DSystem;
using UnityEngine;
using UnityEngine.Splines;

namespace gishadev.golf.Utilities
{
    [RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
    [RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
    [ExecuteInEditMode]
    public class ProceduralSplineBasedFieldGenerator : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameDataSO;

        private SplineContainer _splineContainer;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;

        private void Start() => Initialize();
        private void LateUpdate() => GenerateField();

        private void Initialize()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
            _splineContainer = GetComponent<SplineContainer>();
            _meshFilter = GetComponent<MeshFilter>();

            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = gameDataSO.FieldMaterial;
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
    }
}