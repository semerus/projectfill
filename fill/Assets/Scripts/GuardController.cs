using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardController : MonoBehaviour {

	const float initAngleDelta = 0.001f;

	public GameObject vertexPrefab;
	GameObject guard;
	MapGenerator mapGenerator;
	Vector2[] vertices;

//	private Vector3 mousePos;
//	private Vector3 targetPos;
	private float distance = 30f;

//	ArrayList visibleVertices = new ArrayList();

	GameObject Mesher;

	// Use this for initialization
	void Start () {
		mapGenerator = GameObject.Find("Map Generator").GetComponent<MapGenerator> ();
		vertices = mapGenerator.vertices;

		guard = Instantiate (vertexPrefab) as GameObject;
		guard.name = "guard";

		Mesher = new GameObject ("Mesher");
		Mesher.AddComponent<MeshFilter> ();
		Mesher.AddComponent<MeshRenderer> ();
//		guard.transform.position = new Vector2 (0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = PositionGuard ();
		Vector2[] orderedVertices = ShootRays (targetPos);
//		renderVG (orderedVertices);
	}

	Vector3 PositionGuard () {
		Vector3 mousePos = Input.mousePosition;
		Vector3 targetPos = Camera.main.ScreenToWorldPoint (new Vector3(mousePos.x, mousePos.y, distance));

		guard.transform.position = targetPos;

		return targetPos;
	}

	Vector2[] ShootRays (Vector3 targetPos) {
		Vector2[] orderedVertices = new Vector2[vertices.Length * 3];

		for (int i = 0; i < vertices.Length - 1; i++) {//
			int maxCount = 10000;

			Vector2 standardAngle = (vertices [i] - new Vector2(targetPos.x, targetPos.y));

			Vector2 leftAngle = Quaternion.AngleAxis (initAngleDelta, Vector3.forward) * standardAngle;
			int count = 0;
			float angleDelta = initAngleDelta;
			while (leftAngle == standardAngle) {
//				Debug.Log ("LeftAngle deltaAngle = " + angleDelta);
				transform.Rotate (standardAngle);
				leftAngle = Quaternion.AngleAxis (angleDelta, Vector3.forward) * standardAngle;
				count++;
				angleDelta *= 2;
				if (count >= maxCount) {
					Debug.Log ("LeftAngle passed maxCount");
					break;
				}
			}
				
			Vector2 rightAngle = Quaternion.AngleAxis (-initAngleDelta, Vector3.forward) * leftAngle;
			count = 0;
			angleDelta = initAngleDelta;
			while (rightAngle == standardAngle) {
//				Debug.Log ("RightAngle deltaAngle = " + angleDelta);
				rightAngle = Quaternion.AngleAxis (-angleDelta, Vector3.forward) * standardAngle;
				count++;
				angleDelta *= 2;
				if (count >= maxCount) {
					Debug.Log ("RightAngle passed maxCount");
					break;
				}
			}


//			if (leftAngle == standardAngle || rightAngle == standardAngle) {
//				Debug.Log ("Same");
//			}

//			Debug.Log ("Left, Stand, Right: " + leftAngle + ", " + standardAngle + ", " + rightAngle);

//			if (leftAngle == rightAngle) {
//				Debug.Log ("Left&Right Same" + leftAngle);
//			}

//			if (leftAngle != rightAngle)
//				Debug.Log ("diff " + leftAngle + ", " + standardAngle + ", " + rightAngle);
//			else {
//				Debug.Log ("same " + leftAngle + ", "  + standardAngle + ", " + rightAngle);
//			}

//			drawLine (targetPos, standardAngle, Color.red, Color.white);
//			drawLine (targetPos, leftAngle, Color.red, Color.white);
//			drawLine (targetPos, rightAngle, Color.red, Color.white);

//			RaycastHit2D hit = Physics2D.Raycast (targetPos, standardAngle);
//			RaycastHit2D Lhit = Physics2D.Raycast (targetPos, leftAngle);
//			RaycastHit2D Rhit = Physics2D.Raycast (targetPos, rightAngle);
//
//			Debug.DrawLine (targetPos, Lhit.point, Color.red);
//			Debug.DrawLine (targetPos, Rhit.point, Color.green);
//			Debug.DrawLine (targetPos, hit.point, Color.white);


			RaycastHit2D hit = drawLine (targetPos, leftAngle, Color.green);
			RaycastHit2D leftHit = drawLine (targetPos, rightAngle, Color.blue);
			RaycastHit2D rightHit = drawLine (targetPos, standardAngle);

//			if (hit.point == new Vector2 (0, 0)) {
////				Debug.Log (targetPos + "->" + vertices [i] );
//				Debug.Log (hit.collider + ": " + targetPos + "->" + vertices[i]);
//				for (int j = i; j < vertices.Length - 1; j++) {
//					Debug.DrawLine (targetPos, vertices [j], Color.white);
//				}
//			} else {
//				Debug.Log ("Else: " + hit.collider);
//			}


//			Debug.DrawLine (targetPos, rightHit.point, Color.cyan);

			// add to 2D vertices
			orderedVertices [i * 3 + 0] = leftHit.point;
			orderedVertices [i * 3 + 1] = hit.point;
			orderedVertices [i * 3 + 2] = rightHit.point;
		}

		return orderedVertices;
	}

//	void drawLine (Vector3 targetPos, Vector3 queryAngle, Color c1, Color c2)
	RaycastHit2D drawLine (Vector3 targetPos, Vector3 queryAngle)
	{
		RaycastHit2D hit = Physics2D.Raycast (targetPos, queryAngle);

		if (hit.collider != null)
			Debug.DrawLine (targetPos, hit.point, Color.red);
		else {
			Debug.DrawRay (targetPos, queryAngle, Color.white);
		}

		return hit;
	}

	RaycastHit2D drawLine (Vector3 targetPos, Vector3 queryAngle, Color c)
	{
		RaycastHit2D hit = Physics2D.Raycast (targetPos, queryAngle);

		if (hit.collider != null)
			Debug.DrawLine (targetPos, hit.point, c);
		else {
			Debug.DrawRay (targetPos, queryAngle, Color.white);
		}
		return hit;
	}

	void renderVG(Vector2[] orderedVertices)
	{
		
//		for (int i = 0; i < orderedVertices.Length - 1; i++)
//		{
//			Vector2[] vertices2D = new Vector2[3];
//			vertices2D [0] = orderedVertices[i];
//			vertices2D [1] = guard.transform.position;
//			vertices2D [2] = orderedVertices[i + 1];
//			Debug.Log ("Vertices2D[0] " + vertices2D [0] + ", " + vertices2D [1] + ", " + vertices2D [2]);
//
//			renderOneVG (vertices2D, gObj);
//		}

		/* the reason the last triangle is dealt separatly is because of
		out of bound exception (i.e., using array does not allow wrapping around
		*/

		// destroy already existing triangle

		renderOneVG (vertices, Mesher);
	}

	void renderOneVG(Vector2[] vertices2D, GameObject gObj)
	{

		// Create the Vector3 vertices
		Vector3[] vertices3D = new Vector3[vertices2D.Length];
		for (int i = 0; i < vertices2D.Length; i++) {
			vertices3D [i] = new Vector3 (vertices2D [i].x, vertices2D [i].y, 0);
		}

		List<Vector3> listsV3 = new List<Vector3> ();
		for (int i = 0; i < vertices3D.Length; i++) {
			listsV3.Add (-Vector3.forward);
		}

		Debug.Log (vertices3D.Length);

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		int[] indices = tr.Triangulate ();

		// create polygons
		MeshRenderer mRend = Mesher.GetComponent<MeshRenderer>();
		MeshFilter filter = Mesher.GetComponent<MeshFilter> ();

		mRend.material.color = new Color (1, 0, 0);

		Mesh msh = new Mesh ();
		msh.vertices = vertices3D;
		msh.triangles = indices;
//		msh.RecalculateNormals ();
//		msh.RecalculateBounds ();
		msh.SetNormals (listsV3);

		filter.mesh = msh;
	}
		
}
