using System.Collections.Generic;
using UnityEngine;

namespace HanoiTowers
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Disk : MonoBehaviour
    {
        [SerializeField] float minRadius = 0.5f;
        [SerializeField] float maxRadius = 1.2f;
        [SerializeField] float innerRadius = 0.3f;
        [SerializeField] int numberOfSegments = 32;
        [SerializeField] float height = 0.3f;
        [SerializeField] Material defaultMaterial;
        [SerializeField] Material highlightMaterial;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        List<Vector3> vertices;
        List<int> triangles;
        bool highlight;

        public int MaxDisks { get; set; }
        public Peg Peg { get; set; }
        public int Size { get; set; }
        public float Height => height;

        public bool Highlight
        {
            get => highlight;
            set
            {
                highlight = value;
                meshRenderer.material = value ? highlightMaterial : defaultMaterial;
            }
        }

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }

        void Start()
        {
            Mesh mesh = BuildMesh();
            meshFilter.mesh = mesh;
            meshRenderer.material = defaultMaterial;
        }

        Mesh BuildMesh()
        {
            Mesh mesh = new Mesh();

            Triangulate();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        void Triangulate()
        {
            float alpha = Mathf.PI / numberOfSegments * 2;
            float totalAlpha = 0;
            float stepSize = MaxDisks > 1 ? (maxRadius - minRadius) / (MaxDisks - 1) : 0f;
            float radius = minRadius + stepSize * (Size - 1);

            for (int i = 0; i < numberOfSegments; i++)
            {
                Vector3 c1 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), height,
                    innerRadius * Mathf.Sin(totalAlpha));
                Vector3 v1 = new Vector3(radius * Mathf.Cos(totalAlpha), height, radius * Mathf.Sin(totalAlpha));

                Vector3 c2 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), 0f, innerRadius * Mathf.Sin(totalAlpha));
                Vector3 v2 = new Vector3(radius * Mathf.Cos(totalAlpha), 0f, radius * Mathf.Sin(totalAlpha));

                totalAlpha += alpha;

                Vector3 c3 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), height,
                    innerRadius * Mathf.Sin(totalAlpha));
                Vector3 v3 = new Vector3(radius * Mathf.Cos(totalAlpha), height, radius * Mathf.Sin(totalAlpha));

                Vector3 c4 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), 0f, innerRadius * Mathf.Sin(totalAlpha));
                Vector3 v4 = new Vector3(radius * Mathf.Cos(totalAlpha), 0f, radius * Mathf.Sin(totalAlpha));

                AddQuad(c1, c3, c4, c2);
                AddQuad(c1, v1, v3, c3);
                AddQuad(c2, c4, v4, v2);
                AddQuad(v1, v2, v4, v3);
            }
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            vertices.Add(v3);
            vertices.Add(v2);
            vertices.Add(v1);

            int current = triangles.Count;
            triangles.Add(current + 0);
            triangles.Add(current + 1);
            triangles.Add(current + 2);
        }

        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            AddTriangle(v1, v2, v3);
            AddTriangle(v1, v3, v4);
        }
    }
}