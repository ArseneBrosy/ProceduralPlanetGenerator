using UnityEngine;

public static class SphereGenerator {

    public static Mesh CreateFace(Vector3 normal, int resolution, float scale = 1f) {
        Mesh mesh = new Mesh();

        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = Vector3.Cross(normal, axisA);

        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uvs = new Vector2[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        int triIndex = 0;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                int vertexIndex = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1f);
                Vector3 point = normal + axisA * (percent.x - 0.5f) * 2 + axisB * (percent.y - 0.5f) * 2;
                point = PointOnCubeToPointOnSphere(point);
                Vector2 latLon = (normal == Vector3.up || normal == Vector3.down) ? PointOnSphereToLatLon(new Vector3(-point.y, point.x, point.z)) : PointOnSphereToLatLon(point);
                Vector2 uvPercent = new Vector2((latLon.x + 180f) / 360f, (latLon.y + 90f) / 180f);
                point *= scale;

                if (uvPercent.x < 0.25f && normal == Vector3.back) {
                    uvPercent.x += 1f;
                }

                vertices[vertexIndex] = point;
                uvs[vertexIndex] = uvPercent;

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
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh[] GenerateFaces(int resolution, float scale = 1f) {
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
            allMeshes[i] = SphereGenerator.CreateFace(faceNormals[i], resolution, scale);
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

    public static Vector2 PointOnSphereToLatLon(Vector3 p) {
        float latitude = Mathf.Asin(p.y) * Mathf.Rad2Deg;
        float longitude = Mathf.Atan2(p.x, p.z) * Mathf.Rad2Deg;
        return new Vector2(longitude, latitude);
    }

    public static Vector3 LatLonToPointOnSphere(float latitude, float longitude) {
        float latRad = latitude * Mathf.Deg2Rad;
        float lonRad = longitude * Mathf.Deg2Rad;

        float x = Mathf.Cos(latRad) * Mathf.Sin(lonRad);
        float y = Mathf.Sin(latRad);
        float z = Mathf.Cos(latRad) * Mathf.Cos(lonRad);

        return new Vector3(x, y, z);
    }
}
