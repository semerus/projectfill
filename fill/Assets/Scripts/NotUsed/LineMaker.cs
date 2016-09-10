//using UnityEngine;
//using System.Collections;
//
//public class LineMaker : MonoBehaviour {
//
//	public GameObject linePrefab;
//	GameObject line;
//	Vector3 p0 = new Vector3 (0f, -7f);
//	Vector3 p1 = new Vector3 (8f, -10f);
//	Vector3 p2 = new Vector3 (3f, -3f);
//	Vector3 p3 = new Vector3 (8f, 5f);
//	Vector3 p4 = new Vector3 (3f, 3f);
//	Vector3 p5 = new Vector3 (0f, 10f);
//	Vector3 p6 = new Vector3 (-3f, 3f);
//	Vector3 p7 = new Vector3 (-8f, 5f);
//	Vector3 p8 = new Vector3 (-3f, -3f);
//	Vector3 p9 = new Vector3 (-8f, -10f);
//
//	Vector3[] vertices;
//	int length;
//
//	// Use this for initialization
//	void Start () {
//		vertices = new Vector3[] {p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p0};
//		length = vertices.Length;
//		CreateMap ();
//	}
//	
//	void CreateMap () {
//		GameObject temp = Instantiate (linePrefab) as GameObject;
//		temp.name = "line";
//		LineRenderer lineRenderer = temp.GetComponent<LineRenderer> ();
//		lineRenderer.SetVertexCount(length);
//		lineRenderer.SetWidth (0.2f, 0.2f);
//		lineRenderer.SetPositions(vertices);
//	}
//}
