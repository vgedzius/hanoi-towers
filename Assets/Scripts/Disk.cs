using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Disk : MonoBehaviour
{
    [SerializeField] int size = 1;
    [SerializeField] int maxDisks = 2; // temporary, later game will set this

    [Header("Rendering")] 
    [SerializeField] float minRadius = 0.5f;
    [SerializeField] float maxRadius = 1.2f;
    [SerializeField] float innerRadius = 0.3f;
    [SerializeField] int numberOfSegments = 32;
    [SerializeField] float height = 0.3f;

    MeshFilter meshFilter;
    List<Vector3> vertices;
    List<int> triangles;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    void Start()
    {
        meshFilter.mesh = BuildMesh();
    }

    Mesh BuildMesh()
    {
        Mesh mesh = new Mesh();

        Triangulate(transform.localPosition);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    void Triangulate(Vector3 center)
    {
        float alpha = Mathf.PI / numberOfSegments * 2;
        float totalAlpha = 0;
        float stepSize = (maxRadius - minRadius) / (maxDisks - 1);
        float radius = minRadius + stepSize * (size - 1);

        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 c1 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), height, innerRadius * Mathf.Sin(totalAlpha));
            Vector3 v1 = new Vector3(radius * Mathf.Cos(totalAlpha), height, radius * Mathf.Sin(totalAlpha));

            Vector3 c2 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), 0f, innerRadius * Mathf.Sin(totalAlpha));
            Vector3 v2 = new Vector3(radius * Mathf.Cos(totalAlpha), 0f, radius * Mathf.Sin(totalAlpha));

            totalAlpha += alpha;

            Vector3 c3 = new Vector3(innerRadius * Mathf.Cos(totalAlpha), height, innerRadius * Mathf.Sin(totalAlpha));
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