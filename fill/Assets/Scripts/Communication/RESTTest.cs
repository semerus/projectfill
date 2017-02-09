using System;
using UnityEngine;
using System.Collections;

// Get the latest webcam shot from outside "Friday's" in Times Square
public class RESTTest : MonoBehaviour {
	public string url = "http://127.0.0.1:8080/top_scores";

	IEnumerator Start() {
		// Start a download of the given URL
		WWW www = new WWW(url);

		// Wait for download to complete
		yield return www;

//		// assign texture
//		Renderer renderer = GetComponent<Renderer>();
//		renderer.material.mainTexture = www.texture;
		GUIText text = GetComponent<GUIText>();
		text.text = www.text;
	}
}
