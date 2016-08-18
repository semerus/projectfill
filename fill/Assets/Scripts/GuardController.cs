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
		Vector2[] orderedVertices = ShootRays ();
		renderVG (orderedVertices);
	}

	void PositionGuard () {
		mousePos = Input.mousePosition;
		targetPos = Camera.main.ScreenToWorldPoint (new Vector3(mousePos.x, mousePos.y, distance));

		guard.transform.position = targetPos;
	}

	Vector2[] ShootRays () {
		Vector2[] orderedVertices = new Vector2[numVertices * 3];
		
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

			// add to 2D vertices
			orderedVertices [i * 3 + 0] = leftHit.point;
			orderedVertices [i * 3 + 1] = hit.point;
			orderedVertices [i * 3 + 2] = rightHit.point;
		}

		for (int i = 0; i < orderedVertices.Length; i++) {
			Debug.Log (orderedVertices [i]);
		}

		return orderedVertices;
	}

	void renderVG(Vector2[] orderedVertices)
	{
		for (int i = 0; i < numVertices * 3 - 1; i++) //numVertices * 3 - 1
		{
			// TODO: better solution than destroy and re create?
			Destroy (GameObject.Find ("Triangle" + i));
			GameObject gObj = new GameObject ("Triangle" + i);
			Debug.Log ("Creating " + i + "-th triangle");

			Vector2[] vertices2D = new Vector2[3];
			vertices2D [0] = orderedVertices[i];
			vertices2D [1] = new Vector2(0, 0);
			vertices2D [2] = orderedVertices[i + 1];
//			Debug.Log ("Vertices2D[0] " + vertices2D [0] + ", " + vertices2D [1] + ", " + vertices2D [2]);

			renderOneVG (vertices2D, gObj);
		}

		/* the reason the last triangle is dealt separatly is because of
		out of bound exception (i.e., using array does not allow wrapping around
		*/

		// destroy already existing triangle
		Destroy (GameObject.Find ("FinalTriangle"));

		// create GameObject
		GameObject gObj_2 = new GameObject("FinalTriangle");

		// create vertices2D info
		Vector2[] vertices2D_2 = new Vector2[3];
		vertices2D_2 [0] = orderedVertices[numVertices * 3 - 1];
		vertices2D_2 [1] = new Vector2(0, 0);
		vertices2D_2 [2] = orderedVertices[0];

		// render it
		renderOneVG(vertices2D_2, gObj_2);
	}

	void renderOneVG(Vector2[] vertices2D, GameObject gObj)
	{

		// Create the Vector3 vertices
		Vector3[] vertices3D = new Vector3[3];
		vertices3D [0] = new Vector3 (vertices2D[0].x, vertices2D[0].y, 0);
		vertices3D [1] = new Vector3 (vertices2D[1].x, vertices2D[1].y, 0);
		vertices3D [2] = new Vector3 (vertices2D[2].x, vertices2D[2].y, 0);
		//			Debug.Log ("Vertices3D[0] " + vertices3D [0] + ", " + vertices3D [1] + ", " + vertices3D [2]);

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		int[] indices = tr.Triangulate ();

		// create polygons
		MeshRenderer mRend = gObj.AddComponent<MeshRenderer>();
		MeshFilter filter = gObj.AddComponent<MeshFilter> ();

		mRend.material.color = new Color (1, 0, 0);
		Mesh msh = new Mesh ();
		msh.vertices = vertices3D;
		msh.triangles = indices;
		msh.RecalculateNormals ();
		msh.RecalculateBounds ();

		filter.mesh = msh;
	}
		
}
