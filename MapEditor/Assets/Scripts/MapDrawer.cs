using UnityEngine;
using System.Collections;

public class MapDrawer : MonoBehaviour {
	public Vector2[] vertices;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DrawLine ();
	}

	void DrawLine () {
		Vector2 startLine;
		Vector2 endLine;
		bool drawing = false;
		if (Input.GetMouseButtonUp (0)) {
			drawing = true;
		}
	}
}
