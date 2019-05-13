using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFire : MonoBehaviour {

	[Header("Control")]
	[Range(0.0f, 3.0f)]
	public float updateFrequency = 1.0f;

	[Header("Graphics")]
	public Gradient gradient;
	public Texture texture;

	[Header("Settings")]
	public float size = 30.0f;
	[Range(1, 100)]
	public float tileSize = 20.0f;
	public Vector2 offset = Vector2.zero;

	[Header("Data")]
	public List<float> fireData = new List<float>();

	private float time;

	private int width;
	private int height;
	private int screenSize;

	void Start() {
		width = (int)(Screen.width / tileSize);
		height = (int)(Screen.height / tileSize);
		screenSize = width * height;
		for (int id = 0; id < screenSize; id++) {
			// fire source
			if (id >= screenSize - width) {
				fireData.Add(1.0f);
			}
			else {
				fireData.Add(0.0f);
			}
		}
	}

	// Update is called once per frame
	void Update() {
		time += Time.deltaTime;
		if (time >= updateFrequency) {
			//reset time
			time = 0.0f;
			CalculateFirePropagation();
		}
	}

	void CalculateFirePropagation() {
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				int pixelIndex = x + (width * y);
				int bellowPixelIndex = pixelIndex + width;

				if (bellowPixelIndex >= screenSize) {
					return;
				}

				//float decay = 1.0f / height;
				float decay = Random.Range(0.0f, 4.0f / height);
				float bellowPixelFireIntensity = fireData[bellowPixelIndex];
				float newFireIntensity = bellowPixelFireIntensity - decay;

				fireData[Mathf.RoundToInt(pixelIndex + Mathf.Clamp01(decay * height))] = newFireIntensity;
			}
		}
	}

	void OnGUI() {
		Color guiColor = GUI.color;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (texture) {
					int pixelIndex = x + (width * y);
					//float deltaY = Mathf.InverseLerp(0.0f, height, y);

					GUI.color = gradient.Evaluate(fireData[pixelIndex]);
					GUI.DrawTexture(new Rect(offset.x + x * tileSize, offset.y + y * tileSize, size, size), texture);
				}
			}
		}

		GUI.color = guiColor;
	}
}
