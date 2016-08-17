using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject linePrefab;
	public GameObject vertexPrefab;


	//GameObject line;
	Vector2 p0 = new Vector2 (0f, -7f);
	Vector2 p1 = new Vector2 (8f, -10f);
	Vector2 p2 = new Vector2 (3f, -3f);
	Vector2 p3 = new Vector2 (8f, 5f);
	Vector2 p4 = new Vector2 (3f, 3f);
	Vector2 p5 = new Vector2 (0f, 10f);
	Vector2 p6 = new Vector2 (-3f, 3f);
	Vector2 p7 = new Vector2 (-8f, 5f);
	Vector2 p8 = new Vector2 (-3f, -3f);
	Vector2 p9 = new Vector2 (-8f, -10f);

	public Vector2[] vertices;
	public int length;

	// Use this for initialization
	void Start () {
		vertices = new Vector2[] {p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p0};
		length = vertices.Length - 1;
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
		CreateCollider ();
	}

	//creating a dot sprite for each vertex to render round corners
	void CreateVertex() {
		for (int i = 0; i < length; i++) {
			GameObject vertex = Instantiate (vertexPrefab) as GameObject;
			vertex.name = "vertex" + i;
			vertex.transform.position = vertices [i];
		}
	}

	//creating EdgeCollider2D by connecting the vertices of the polygon
	void CreateCollider () {
		GameObject edge = new GameObject ("Edge Collider");
		edge.AddComponent<EdgeCollider2D> ();
		EdgeCollider2D edgeCollider = edge.GetComponent<EdgeCollider2D> ();

		edgeCollider.points = vertices;
	}
}
