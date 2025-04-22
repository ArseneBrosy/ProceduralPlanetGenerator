using UnityEngine;

public static class Noise {

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
