using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject linePrefab;
	public GameObject vertexPrefab;
	GameObject line;
	Vector3 p0 = new Vector3 (0f, -7f);
	Vector3 p1 = new Vector3 (8f, -10f);
	Vector3 p2 = new Vector3 (3f, -3f);
	Vector3 p3 = new Vector3 (8f, 5f);
	Vector3 p4 = new Vector3 (3f, 3f);
	Vector3 p5 = new Vector3 (0f, 10f);
	Vector3 p6 = new Vector3 (-3f, 3f);
	Vector3 p7 = new Vector3 (-8f, 5f);
	Vector3 p8 = new Vector3 (-3f, -3f);
	Vector3 p9 = new Vector3 (-8f, -10f);
	//Vector3 p10 = new Vector3 (0f, 3f);

	Vector3[] vertices;
	int length;

	// Use this for initialization
	void Start () {
		vertices = new Vector3[] {p0, p1, p2, p3, p4, p5, p6, p7, p8, p9};
		length = vertices.Length;
		CreateMap ();
	}

	//creating map using the vertices
	void CreateMap () {
		for (int i = 0; i < length; i++) {
			GameObject temp = Instantiate (linePrefab) as GameObject;
			temp.name = "line"+i;
			LineRenderer lineRenderer = temp.GetComponent<LineRenderer> ();
			Vector3[] positions = new Vector3[2];
			if (i < length - 1) {
				positions [0] = vertices [i];
				positions [1] = vertices [i + 1];
			} else {
				positions [0] = vertices [i];
				positions [1] = vertices [0];
			}
			lineRenderer.SetPositions(positions);
		}
		CreateVertex ();
	}

	void CreateVertex() {
		for (int i = 0; i < length; i++) {
			GameObject vertex = Instantiate (vertexPrefab) as GameObject;
			vertex.name = "vertex" + i;
			vertex.transform.position = vertices [i];
		}
	}
	/*
	void AddColliderToLine() {
		BoxCollider2D col = new GameObject ("Collider").AddComponent<BoxCollider2D> ();
		col.transform.parent = 
	}
	*/
}
