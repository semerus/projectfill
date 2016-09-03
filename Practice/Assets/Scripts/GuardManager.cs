using UnityEngine;
using System.Collections;

public class GuardManager : MonoBehaviour {

	const float distance = 10f;

	MapGenerator mg;
	private Vector2[] vertices;


	public GameObject guard;

	// Use this for initialization
	void Start () {
		guard = new GameObject ("guard");
		mg = GameObject.Find ("MapGenerator").GetComponent<MapGenerator> ();
		vertices = mg.vertices;
		Debug.Log (vertices [1]);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));

		guard.transform.position = mousePos;
		ShootRays ();
	}

	void ShootRays() {
		for(int i = 0; i < vertices.Length; i++) {
			RaycastHit2D hit = Physics2D.Linecast(guard.transform.position, vertices[i]);
			Debug.DrawLine (guard.transform.position, hit.point, Color.red);
			Debug.Log (hit.point);
		}
	}
}
