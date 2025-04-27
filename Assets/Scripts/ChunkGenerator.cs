using UnityEngine;

public static class ChunkGenerator {

    public static int facesResolution;
    public static float planetScale = 1f;

    public static Mesh GenerateChunk(int posX, int posY, Vector3 normal, int resolution) {
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
                Vector2 percent = new Vector2(x, y) / (resolution - 1f) / facesResolution + new Vector2(posX, posY) / (float)facesResolution;
                Vector3 point = normal + axisA * (percent.x - 0.5f) * 2 + axisB * (percent.y - 0.5f) * 2;
                point = SphereGenerator.PointOnCubeToPointOnSphere(point);
                Vector2 latLon = (normal == Vector3.up || normal == Vector3.down) ? SphereGenerator.PointOnSphereToLatLon(new Vector3(-point.y, point.x, point.z)) : SphereGenerator.PointOnSphereToLatLon(point);
                Vector2 uvPercent = new Vector2((latLon.x + 180f) / 360f, (latLon.y + 90f) / 180f);
                point *= planetScale;

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
}
