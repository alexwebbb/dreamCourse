// Builds a Mesh containing a single triangle with uvs.
// Create arrays of vertices, uvs and triangles, and copy them into the mesh.

using UnityEngine;

public class MeshTest : MonoBehaviour {

    // Use this for initialization
    void Start() {

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        // make changes to the Mesh by creating arrays which contain the new values
        mesh.vertices = GenerateVertices(10, 10);
        mesh.uv = GenerateUv(10, 10);
        mesh.triangles = GenerateTriangles(10, 10);
    }

    private Vector2[] GenerateUv(int width, int height) {
        Vector2[] uv = new Vector2[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                uv[y * width + x] = new Vector2(x / (float)width, y / (float)height);
            }
        }
        return uv;
    }

    private int[] GenerateTriangles(int width, int height) {
        //generate two triangles per vertex except the last column and last row
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        for (int y = 0; y < height - 1; y++) {
            for (int x = 0; x < width - 1; x++) {
                triangles[(y * (width - 1) + x) * 6] = y * width + x;
                triangles[(y * (width - 1) + x) * 6 + 1] = y * width + x + 1;
                triangles[(y * (width - 1) + x) * 6 + 2] = y * width + x + 1 + width;
                triangles[(y * (width - 1) + x) * 6 + 3] = y * width + x;
                triangles[(y * (width - 1) + x) * 6 + 4] = y * width + x + 1 + width;
                triangles[(y * (width - 1) + x) * 6 + 5] = y * width + x + width;
            }
        }
        return triangles;
    }

    Vector3[] GenerateVertices(int width, int height) {
        Vector3[] vertices = new Vector3[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                vertices[y * width + x] = new Vector3(x / (float)width, y / (float)height);
            }
        }
        return vertices;
    }
}