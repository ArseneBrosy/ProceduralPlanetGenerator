using UnityEngine;
using System.Collections.Generic;

public class PlanetGenerator : MonoBehaviour {

    [Min(1)]
    public int resolution = 10;
    public Material faceMaterial;
    public Transform planet;

    [Space]
    public int seed = 0;

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

        // generate the sphere
        Mesh[] faces = SphereGenerator.GenerateFaces(resolution + 1);

        // create the meshes
        for (int i = 0; i < faces.Length; i++) {
            GameObject faceObj = new GameObject("Face " + i);
            faceObj.transform.parent = planet;

            MeshFilter meshFilter = faceObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = faceObj.AddComponent<MeshRenderer>();

            meshFilter.mesh = faces[i];
            meshRenderer.material = faceMaterial != null ? faceMaterial : new Material(Shader.Find("Standard"));
        }
    }
}
