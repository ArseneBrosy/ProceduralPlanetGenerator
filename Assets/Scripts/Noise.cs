using UnityEngine;

public static class Noise {

	public static float[,] GenerateFractalNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float scaleMultiplier, float influenceMultipier) {
		if (octaves <= 0) {
			octaves = 1;
		}

		float octaveScale = scale;
		float influence = 1f;
		float maxInfluence = influence;
		float[,] finalNoiseMap = new float[mapWidth, mapHeight];

		for (int i = 0; i < octaves; i++) {
			float[,] noiseMap = GenerateNoiseMap(mapWidth, mapHeight, octaveScale);
			noiseMap = MultiplyFloatArray(noiseMap, influence);
			octaveScale *= scaleMultiplier;
			influence *= influenceMultipier;
			maxInfluence += influence;

			// add the new noise map on top
			if (i == 0) {
				finalNoiseMap = noiseMap;
			} else {
				finalNoiseMap = AddFloatArrays(finalNoiseMap, noiseMap);
			}
        }

		// normalize the map
		finalNoiseMap = MultiplyFloatArray(finalNoiseMap, 1 / maxInfluence);

		return finalNoiseMap;
    }

	public static float[,] AddFloatArrays(float[,] array1, float[,] array2) {
		int rows = array1.GetLength(0);
		int cols = array1.GetLength(1);

		if (array2.GetLength(0) != rows || array2.GetLength(1) != cols) {
			throw new System.ArgumentException("Not the same array sizes");
		}

		float[,] result = new float[rows, cols];

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				result[i, j] = array1[i, j] + array2[i, j];
			}
		}

		return result;
	}

	public static float[,] MultiplyFloatArray(float[,] array, float m) {
		int rows = array.GetLength(0);
		int cols = array.GetLength(1);

		float[,] result = new float[rows, cols];

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				result[i, j] = array[i, j] * m;
			}
		}

		return result;
	}

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale) {
		float[,] noiseMap = new float[mapWidth, mapHeight];

		if (scale <= 0) {
			scale = 0.0001f;
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				float latitude = Mathf.Lerp(90, -90, y / (float)mapHeight);
				float longitude = Mathf.Lerp(-180, 180, x / (float)mapWidth);
				Vector3 pointOnSphere = SphereGenerator.LatLonToPointOnSphere(latitude, longitude);
				float sampleX = pointOnSphere.x / scale;
				float sampleY = pointOnSphere.y / scale;
				float sampleZ = pointOnSphere.z / scale;

				float perlinValue = PerlinNoise3D(sampleX, sampleY, sampleZ);
				noiseMap[x, y] = perlinValue;
			}
		}

		return noiseMap;
	}

	public static float PerlinNoise3D(float x, float y, float z) {
		x += 4856f;
		y += 1f;
		z += 2f;
		float xy = _perlin3DFixed(x, y);
		float xz = _perlin3DFixed(x, z);
		float yz = _perlin3DFixed(y, z);
		float yx = _perlin3DFixed(y, x);
		float zx = _perlin3DFixed(z, x);
		float zy = _perlin3DFixed(z, y);

		return xy * xz * yz * yx * zx * zy;
	}

	static float _perlin3DFixed(float a, float b) {
		return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
	}
}
