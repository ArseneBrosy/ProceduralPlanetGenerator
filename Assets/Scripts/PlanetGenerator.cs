using UnityEngine;
using System.Collections.Generic;

public class PlanetGenerator : MonoBehaviour {

    [Min(1)]
    public int resolution = 10;
    [Min(1)]
    public float scale = 1f;
    public Material faceMaterial;
    public Transform planet;

    [Space]
    public int seed = 0;
    [Min(0)]
    public float noiseScale = 1f;
    [Min(1)]
    public int octaves = 1;
    [Range(0f, 1f)]
    public float scaleMultiplier = 0.3f;
    [Range(0f, 1f)]
    public float influenceMultiplier = 0.3f;

    [Space]
    public bool autoGenerate;

    public void GeneratePlanet() {
        // destroy old planet
        List<Transform> children = new List<Transform>();
        foreach (Transform child in planet.transform) {
            children.Add(child);
        }

        foreach (Transform child in children) {
            DestroyImmediate(child.gameObject);
        }

        // generate new seed
        int newSeed = Noise.GenerateNewSeed(seed);

        // generate the sphere
        Mesh[] faces = SphereGenerator.GenerateFaces(resolution + 1, scale);
        Renderer[] facesRenderers = new Renderer[6];

        // create the meshes
        for (int i = 0; i < faces.Length; i++) {
            GameObject faceObj = new GameObject("Face " + i);
            faceObj.transform.parent = planet;

            MeshFilter meshFilter = faceObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = faceObj.AddComponent<MeshRenderer>();
            MeshCollider meshCollider = faceObj.AddComponent<MeshCollider>();

            meshFilter.mesh = faces[i];
            meshCollider.sharedMesh = faces[i];
            meshRenderer.material = faceMaterial != null ? faceMaterial : new Material(Shader.Find("Standard"));

            facesRenderers[i] = meshRenderer;
        }

        // generate noise map
        float[,] noiseMap = Noise.GenerateFractalNoiseMap(720, 360, noiseScale, newSeed, octaves, scaleMultiplier, influenceMultiplier);
        float[,] noiseMap90 = Noise.GenerateFractalNoiseMap(720, 360, noiseScale, newSeed, octaves, scaleMultiplier, influenceMultiplier, true);

        /*float[,] mask = new float[360, 180];
        int size = 100;
        for (int x = -size; x <= size; x++) {
            for (int y = -size; y <= size; y++) {
                Vector3 point = new Vector3((float)x / size, (float)y / size, 1);
                point = SphereGenerator.PointOnCubeToPointOnSphere(point);
                Vector2 latLon = SphereGenerator.PointOnSphereToLatLon(point);
                latLon.x += 180f;
                latLon.y += 90f;
                mask[(int)Mathf.Floor(latLon.x) % 360, (int)Mathf.Floor(latLon.y) % 180] = 1f;
            }
        }*/

        PlanetDisplay display = FindFirstObjectByType<PlanetDisplay>();
        //display.DrawNoiseMap(mask); // plane
        display.DrawTexture(noiseMap, noiseMap90, facesRenderers);

        // chunks
        ChunkGenerator.facesResolution = resolution;
        ChunkGenerator.planetScale = scale;

        /*Mesh chunk = ChunkGenerator.GenerateChunk(0, 0, Vector3.up, 3);

        GameObject chunkObj = new GameObject("Test Chunk");

        MeshFilter chunkMeshFilter = chunkObj.AddComponent<MeshFilter>();
        MeshRenderer chunkMeshRenderer = chunkObj.AddComponent<MeshRenderer>();

        chunkMeshFilter.mesh = chunk;*/
    }
}
