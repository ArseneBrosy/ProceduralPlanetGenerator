using UnityEngine;

public class PlanetDisplay : MonoBehaviour {
	public Renderer textureRenderer;

	public void DrawNoiseMap(float[,] noiseMap) {
		int width = noiseMap.GetLength(0);
		int height = noiseMap.GetLength(1);

		Texture2D texture = new Texture2D(width, height);

		Color[] colorMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
			}
		}
		texture.SetPixels(colorMap);
		texture.Apply();

		Material newMat = new Material(textureRenderer.sharedMaterial);
		texture.wrapMode = TextureWrapMode.Repeat;
		newMat.mainTexture = texture;
		textureRenderer.sharedMaterial = newMat;
		textureRenderer.transform.localScale = new Vector3(width, 1, height);
	}

	public void DrawTexture(float[,] noiseMap, float[,] noiseMap90, Renderer[] textureRenderers) {
		int width = noiseMap.GetLength(0);
		int height = noiseMap.GetLength(1);

		for (int i = 0; i < textureRenderers.Length; i++) {
			Texture2D texture = new Texture2D(width, height);

			Color[] colorMap = new Color[width * height];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, (i < 2) ? noiseMap90[x, y] : noiseMap[x, y]);
				}
			}
			texture.SetPixels(colorMap);
			texture.Apply();

			Material newMat = new Material(textureRenderers[i].sharedMaterial);
			newMat.mainTexture = texture;
			textureRenderers[i].sharedMaterial = newMat;
		}
	}
}
