using System.Collections.Generic;
using UnityEngine;

namespace HanoiTowers
{
    [RequireComponent(typeof(MeshFilter)), ExecuteInEditMode]
    public class DiskMesh : MonoBehaviour
    {
        [SerializeField] float minRadius = 0.2f;
        [SerializeField] float maxRadius = 1.2f;
        [SerializeField] float innerRadius = 0.15f;
        [SerializeField] float height = 0.2f;
        [SerializeField] int numberOfSegments = 128;

        readonly List<Vector3> vertices = new List<Vector3>();
        readonly List<int> triangles = new List<int>();
        Mesh mesh;
        bool needUpdate = false;
        int scheduledSize;
        int scheduledMaxDisks;

        public float Height => height;

        void Awake()
        {
            mesh = new Mesh {name = "Disk"};
        }

        void Start()
        {
#if UNITY_EDITOR
            UpdateEditor();
#endif
        }

        void Update()
        {
            if (!needUpdate) return;

            Build(scheduledSize, scheduledMaxDisks);

            needUpdate = false;
        }

        public void ScheduleBuild(int buildSize, int buildMaxDisks)
        {
            scheduledSize = buildSize;
            scheduledMaxDisks = buildMaxDisks;
            needUpdate = true;
        }

#if UNITY_EDITOR
        void UpdateEditor()
        {
            if (Application.isPlaying) return;

            ScheduleBuild(5, 10);
        }
#endif

        public void Build(int size, int maxDisks)
        {
            vertices.Clear();
            triangles.Clear();

            Triangulate(size, maxDisks);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        void Triangulate(int size, int maxDisks)
        {
            float alpha = Mathf.PI / numberOfSegments * 2;
            float totalAlpha = 0;
            float stepSize = maxDisks > 1 ? (maxRadius - minRadius) / (maxDisks - 1) : 0f;
            float radius = minRadius + stepSize * (size - 1);

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