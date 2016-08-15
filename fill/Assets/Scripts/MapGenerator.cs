using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject linePrefab;
	public GameObject line;
	Vector3 p0 = new Vector3 (6f, 2f);
	Vector3 p1 = new Vector3 (11f, 6f);
	Vector3 p2 = new Vector3 (5f, 12f);
	Vector3 p3 = new Vector3 (1f, 10f);
	Vector3 p4 = new Vector3 (5f, 7f);

	Vector3[] vertices;
	int length;

	// Use this for initialization
	void Start () {
		vertices = new Vector3[] {p0, p1, p2, p3, p4};
		length = vertices.Length;
		CreateMap ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
	}
}
