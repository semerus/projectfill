using UnityEngine;
using System.Collections;

public class GuardController : MonoBehaviour {

	const float fovAngle = 0.00001f;

	public GameObject vertexPrefab;
	GameObject guard;
	MapGenerator mapGenerator;
	Vector2[] vertices;
	int numVertices;

	private Vector3 mousePos;
	private Vector3 targetPos;
	private float distance = 10f;

	ArrayList visibleVertices = new ArrayList();

	// Use this for initialization
	void Start () {
		mapGenerator = GameObject.Find("Map Generator").GetComponent<MapGenerator> ();
		vertices = mapGenerator.vertices;
		numVertices = vertices.Length - 1;

		guard = Instantiate (vertexPrefab) as GameObject;
		guard.name = "guard";
		guard.transform.position = new Vector2 (0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		PositionGuard ();
		ShootRays ();
	}

	void PositionGuard () {
		mousePos = Input.mousePosition;
		targetPos = Camera.main.ScreenToWorldPoint (new Vector3(mousePos.x, mousePos.y, distance));

		guard.transform.position = targetPos;
	}

	void ShootRays () {
		for (int i = 0; i < numVertices; i++) {
			Vector3 standardAngle = vertices [i];
			Vector3 leftAngle = Quaternion.AngleAxis (-fovAngle, transform.up) * standardAngle;
			Vector3 rightAngle = Quaternion.AngleAxis (fovAngle, transform.up) * standardAngle;

			RaycastHit2D leftHit = Physics2D.Linecast (targetPos, leftAngle);
			RaycastHit2D hit = Physics2D.Linecast (targetPos, vertices [i]);
			RaycastHit2D rightHit = Physics2D.Linecast (targetPos, rightAngle);

			visibleVertices.Add (leftHit.point);
			visibleVertices.Add (hit.point);
			visibleVertices.Add (rightHit.point);

			Debug.DrawLine (targetPos, leftHit.point, Color.red);
			Debug.DrawLine (targetPos, hit.point, Color.red);
			Debug.DrawLine (targetPos, rightHit.point, Color.red);
		}

	}
}
