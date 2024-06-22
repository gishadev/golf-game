using System.Linq;
using gishadev.golf.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace gishadev.golf.Utilities
{
    [RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralSplineBasedMeshGenerator : MonoBehaviour
    {
        [Inject] private GameDataSO _gameDataSO;

        private SplineContainer _splineContainer;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private void Initialize()
        {
            _splineContainer = GetComponent<SplineContainer>();
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        // Knots are vertices
        [Button]
        private void GenerateMesh()
        {
            Initialize();

            var mesh = new Mesh
            {
                name = "Procedural Mesh"
            };

            var knots = _splineContainer.Splines[0].Knots.ToArray();
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
    }
}