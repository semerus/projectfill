//using UnityEngine;
//using System.Collections;
//
//public class EdgeColliderMaker : MonoBehaviour {
//
//	public GameObject edgeColliderPrefab;
//
//	// Use this for initialization
//	void Start () {
//		CreateCollider ();
//	}
//
//	void CreateCollider () {
//		GameObject edge = Instantiate (edgeColliderPrefab) as GameObject;
//		MapGenerator mapGenerator = GameObject.Find ("Map Generator").GetComponent<MapGenerator> ();
//		Vector2[] vertices = mapGenerator.vertices;
//		EdgeCollider2D edgeCollider = edge.GetComponent<EdgeCollider2D> ();
//		edgeCollider.points = vertices;
//	}
//}
