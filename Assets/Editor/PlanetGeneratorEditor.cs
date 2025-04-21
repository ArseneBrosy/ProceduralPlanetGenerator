using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PlanetGenerator))]
public class PlanetGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        PlanetGenerator planGen = (PlanetGenerator)target;

        if (DrawDefaultInspector() && planGen.autoGenerate) {
            planGen.GeneratePlanet();
        }

        if (GUILayout.Button("Generate")) {
            planGen.GeneratePlanet();
        }
    }
}
