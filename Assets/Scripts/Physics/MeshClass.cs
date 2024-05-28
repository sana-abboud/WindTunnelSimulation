using UnityEngine;
using System.Collections.Generic;
public class MeshClass : MonoBehaviour
{
    public List<Triangles> meshTriangles = new List<Triangles>();
    public float triangleScale = 0.1f; // Scale factor for the triangle visualization

    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;

        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int vertexIndex1 = triangles[i];
            int vertexIndex2 = triangles[i + 1];
            int vertexIndex3 = triangles[i + 2];

            Vector3 vertex1 = transform.TransformPoint(vertices[vertexIndex1]);
            Vector3 vertex2 = transform.TransformPoint(vertices[vertexIndex2]);
            Vector3 vertex3 = transform.TransformPoint(vertices[vertexIndex3]);

            Debug.Log("vertex1:" + vertex1);
            Debug.Log("vertex2:" + vertex2);
            Debug.Log("vertex3:" + vertex3);


            // Calculate the center position of the triangle
            Vector3 triangleCenter = (vertex1 + vertex2 + vertex3) / 3f;
            Debug.Log("triangle Center:" + triangleCenter);


            meshTriangles.Add(new Triangles(vertex1, vertex2, vertex3));
            Debug.Log("meshTriangles:" + meshTriangles);

        }
    }


}

