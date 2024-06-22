using System.Linq;
using gishadev.golf.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace gishadev.golf.Utilities
{
    [RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
    [RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
    public class ProceduralSplineBasedFieldGenerator : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameDataSO;

        private SplineContainer _splineContainer;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;

        private void Initialize()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
            _splineContainer = GetComponent<SplineContainer>();
            _meshFilter = GetComponent<MeshFilter>();

            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = gameDataSO.FieldMaterial;
        }

        [Button]
        private void GenerateField()
        {
            Initialize();
            
            var knots = _splineContainer.Splines[0].Knots.ToArray();
            GenerateMesh(knots);
            GenerateEdge(knots);
        } 
        
        private void GenerateMesh(BezierKnot[] knots)
        {
            var mesh = new Mesh
            {
                name = "Procedural Mesh"
            };

            var vertices = knots
                .Select(x => (Vector3) x.Position)
                .ToArray();

            mesh.vertices = vertices;
            mesh.triangles = GenerateTriangles(vertices.Length);
            _meshFilter.mesh = mesh;
        }

        private int[] GenerateTriangles(int vertexCount)
        {
            // Assume a simple shape where vertices are connected in a fan pattern
            int triangleCount = (vertexCount - 2) * 3;
            int[] triangles = new int[triangleCount];

            for (int i = 0; i < vertexCount - 2; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            return triangles;
        }

        private void GenerateEdge(BezierKnot[] knots)
        {
            _lineRenderer.positionCount = knots.Length;
            
            var positions = knots.Select(x => (Vector3) x.Position).ToArray();
            _lineRenderer.SetPositions(positions);
            _edgeCollider.points = positions.Select(x => (Vector2) x).ToArray();
        }
    }
}