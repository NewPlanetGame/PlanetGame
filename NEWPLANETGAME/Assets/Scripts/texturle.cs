using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class texturle : MonoBehaviour {
public int resolution = 2;
private Texture2D texture;

	// Use this for initialization
	private void Start () {
		texture =  new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Point;
		texture.name = "Procedural Texture";
		GetComponent<MeshRenderer>().material.mainTexture = texture;
		FillTexture();

	}
		private void Update () {
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}
	}
	public void FillTexture () {
		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
				texture.SetPixel(x, y, Color.white * Noise.Value(point));
			}
		}
		texture.Apply();
	}

	public static class Noise {
	public static float Value (Vector3 point) {
		int i = (int)point.x;
		return i % 2;
	}
	}

}

