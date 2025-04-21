using UnityEngine;

public static class SphereGenerator {

    public static Mesh[] CreateFace(Vector3 normal, int resolution) {
        Mesh[] allMeshes = new Mesh[resolution * resolution];

        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = Vector3.Cross(normal, axisA);

        for (int y = 0; y < resolution - 1; y++) {
            for (int x = 0; x < resolution - 1; x++) {
                // Calcul des points (comme avant)
                Vector2 percent00 = new Vector2(x, y) / (resolution - 1f);
                Vector2 percent10 = new Vector2(x + 1, y) / (resolution - 1f);
                Vector2 percent01 = new Vector2(x, y + 1) / (resolution - 1f);
                Vector2 percent11 = new Vector2(x + 1, y + 1) / (resolution - 1f);

                Vector3 v00 = normal + axisA * (percent00.x - 0.5f) * 2 + axisB * (percent00.y - 0.5f) * 2;
                Vector3 v10 = normal + axisA * (percent10.x - 0.5f) * 2 + axisB * (percent10.y - 0.5f) * 2;
                Vector3 v01 = normal + axisA * (percent01.x - 0.5f) * 2 + axisB * (percent01.y - 0.5f) * 2;
                Vector3 v11 = normal + axisA * (percent11.x - 0.5f) * 2 + axisB * (percent11.y - 0.5f) * 2;
                v00 = PointOnCubeToPointOnSphere(v00);
                v10 = PointOnCubeToPointOnSphere(v10);
                v01 = PointOnCubeToPointOnSphere(v01);
                v11 = PointOnCubeToPointOnSphere(v11);

                // Création du mesh pour ce quad
                Mesh mesh = new Mesh();
                Vector3[] vertices = new Vector3[4] { v00, v10, v01, v11 };
                int[] triangles = new int[6]
                {
                0, 3, 2,  // triangle 1
                0, 1, 3   // triangle 2
                };

                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.RecalculateNormals();

                allMeshes[y * resolution + x] = mesh;
            }
        }

        return allMeshes;
    }

    public static Mesh[] GenerateFaces(int resolution) {
        int resolution2 = resolution * resolution;
        Mesh[] allMeshes = new Mesh[6 * resolution2];

        Vector3[] faceNormals = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < faceNormals.Length; i++) {
            Mesh[] faceMeshes = new Mesh[resolution2];
            faceMeshes = SphereGenerator.CreateFace(faceNormals[i], resolution);
            for (int j = 0; j < resolution2; j++) {
                allMeshes[i * resolution2 + j] = faceMeshes[j];
            }
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
