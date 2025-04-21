using UnityEngine;

public static class SphereGenerator {

    public static Mesh CreateFace(Vector3 normal, int resolution) {
        Mesh mesh = new Mesh();

        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = Vector3.Cross(normal, axisA);

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        int triIndex = 0;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                int vertexIndex = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1f);
                Vector3 point = normal + axisA * (percent.x - 0.5f) * 2 + axisB * (percent.y - 0.5f) * 2;
                point = PointOnCubeToPointOnSphere(point);

                vertices[vertexIndex] = point;

                if (x != resolution - 1 && y != resolution - 1) {
                    int i0 = vertexIndex;
                    int i1 = vertexIndex + 1;
                    int i2 = vertexIndex + resolution;
                    int i3 = vertexIndex + resolution + 1;

                    triangles[triIndex + 0] = i0;
                    triangles[triIndex + 1] = i3;
                    triangles[triIndex + 2] = i2;

                    triangles[triIndex + 3] = i0;
                    triangles[triIndex + 4] = i1;
                    triangles[triIndex + 5] = i3;

                    triIndex += 6;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh[] GenerateFaces(int resolution) {
        Mesh[] allMeshes = new Mesh[6];

        Vector3[] faceNormals = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < faceNormals.Length; i++) {
            allMeshes[i] = SphereGenerator.CreateFace(faceNormals[i], resolution);
        }

        return allMeshes;
    }

    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p) {
        float x = p.x;
        float y = p.y;
        float z = p.z;

        float x2 = x * x;
        float y2 = y * y;
        float z2 = z * z;

        float newX = x * Mathf.Sqrt(1f - (y2 / 2f) - (z2 / 2f) + (y2 * z2) / 3f);
        float newY = y * Mathf.Sqrt(1f - (z2 / 2f) - (x2 / 2f) + (z2 * x2) / 3f);
        float newZ = z * Mathf.Sqrt(1f - (x2 / 2f) - (y2 / 2f) + (x2 * y2) / 3f);

        return new Vector3(newX, newY, newZ);
    }
}
