using UnityEngine;
using System.Collections;

public class GuardController : MonoBehaviour {

	const float initAngleDelta = 0.00001f;

	public GameObject vertexPrefab;
	GameObject guard;
	MapGenerator mapGenerator;
	Vector2[] vertices;

//	private Vector3 mousePos;
//	private Vector3 targetPos;
	private float distance = 30f;

//	ArrayList visibleVertices = new ArrayList();


	// Use this for initialization
	void Start () {
		mapGenerator = GameObject.Find("Map Generator").GetComponent<MapGenerator> ();
		vertices = mapGenerator.vertices;

		guard = Instantiate (vertexPrefab) as GameObject;
		guard.name = "guard";
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

//		string temp = "";
//		for (int i = 0; i < vertices.Length - 1; i++) {
//			temp += vertices[i] + "";
//		}
//		Debug.Log (temp);
//
//		Debug.Log (temp);
//		string temp2 = "";
//		for (int i = 0; i < vertices.Length - 1; i++) {
////			RaycastHit2D[] hits = Physics2D.LinecastAll (targetPos, vertices [i]);
////			for (int j = 0; j < hits.Length; j++) {
////				Debug.Log (Physics.Linecast (targetPos, hits [j].point));
////				Debug.DrawLine (new Vector2(targetPos.x, targetPos.y), hits [j].point);
////			}
//			RaycastHit2D hit = Physics2D.Linecast (targetPos, vertices [i]);
//			Debug.DrawLine (targetPos, hit.point, Color.red);
////			Debug.DrawLine (targetPos, vertices[i], Color.white);
//			temp2 += hit.point;
//		}
//		Debug.Log (temp2);

		for (int i = 0; i < vertices.Length - 1; i++) {
			int maxCount = 10000;

			Vector2 standardAngle = (vertices [i] - new Vector2(targetPos.x, targetPos.y)).normalized;
			Vector2 leftAngle = Quaternion.AngleAxis (initAngleDelta, Vector3.up) * standardAngle;
			int count = 0;
			float angleDelta = initAngleDelta;
			while (leftAngle == standardAngle) {
				leftAngle = Quaternion.AngleAxis (angleDelta, Vector3.up) * leftAngle;
				count++;
				angleDelta *= 2;
				if (count >= maxCount) {
					Debug.Log ("LeftAngle passed maxCount");
					break;
				}
			}

			Vector2 rightAngle = Quaternion.AngleAxis (-initAngleDelta, Vector3.up) * leftAngle;
			count = 0;
			angleDelta = initAngleDelta;
			while (rightAngle == standardAngle) {
				rightAngle = Quaternion.AngleAxis (-angleDelta, Vector3.up) * rightAngle;
				count++;
				angleDelta *= 2;
				if (count >= maxCount) {
					Debug.Log ("RightAngle passed maxCount");
					break;
				}
			}

			if (leftAngle == standardAngle || rightAngle == standardAngle) {
				Debug.Log ("Same");
			}

//			if (leftAngle != rightAngle)
//				Debug.Log ("diff " + leftAngle + ", " + standardAngle + ", " + rightAngle);
//			else {
//				Debug.Log ("same " + leftAngle + ", "  + standardAngle + ", " + rightAngle);
//			}

//			drawLine (targetPos, standardAngle, Color.red, Color.white);
//			drawLine (targetPos, leftAngle, Color.red, Color.white);
//			drawLine (targetPos, rightAngle, Color.red, Color.white);

			drawLine (targetPos, standardAngle);
			drawLine (targetPos, leftAngle);
			drawLine (targetPos, rightAngle);

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
//			orderedVertices [i * 3 + 0] = leftHit.point;
//			orderedVertices [i * 3 + 1] = hit.point;
//			orderedVertices [i * 3 + 2] = rightHit.point;
		}

		return orderedVertices;
	}

//	void drawLine (Vector3 targetPos, Vector3 queryAngle, Color c1, Color c2)
	void drawLine (Vector3 targetPos, Vector3 queryAngle)
	{
		//			RaycastHit2D leftHit = Physics2D.Linecast (targetPos, leftAngle);
		RaycastHit2D hit = Physics2D.Raycast (targetPos, queryAngle);
		//			RaycastHit2D rightHit = Physics2D.Linecast (targetPos, rightAngle);

		//			Debug.DrawLine (targetPos, leftHit.point, Color.green);
		if (hit.collider != null)
			Debug.DrawLine (targetPos, hit.point, Color.red);
		else
			Debug.DrawRay (targetPos, queryAngle, Color.white);
	}
}

//	void renderVG(Vector2[] orderedVertices)
//	{
//		for (int i = 0; i < orderedVertices.Length - 1; i++)
//		{
//			// TODO: better solution than destroy and re create?
//			Destroy (GameObject.Find ("Triangle" + i);
//			GameObject gObj = new GameObject ("Triangle" + i);
////			Debug.Log ("Creating " + i + "-th triangle");
//
//			Vector2[] vertices2D = new Vector2[3];
//			vertices2D [0] = orderedVertices[i];
//			vertices2D [1] = guard.transform.position;
//			vertices2D [2] = orderedVertices[i + 1];
//			Debug.Log ("Vertices2D[0] " + vertices2D [0] + ", " + vertices2D [1] + ", " + vertices2D [2]);
//
//			renderOneVG (vertices2D, gObj);
//		}
//
//		/* the reason the last triangle is dealt separatly is because of
//		out of bound exception (i.e., using array does not allow wrapping around
//		*/
//
//		// destroy already existing triangle
//		Destroy (GameObject.Find ("FinalTriangle"));
//
//		// create GameObject
//		GameObject gObj_2 = new GameObject("FinalTriangle");
//
//		// create vertices2D info
//		Vector2[] vertices2D_2 = new Vector2[3];
//		vertices2D_2 [0] = orderedVertices[orderedVertices.Length - 1];
//		vertices2D_2 [1] = guard.transform.position;
//		vertices2D_2 [2] = orderedVertices[0];
//
//		// render it
//		renderOneVG(vertices2D_2, gObj_2);
//	}
//
//	void renderOneVG(Vector2[] vertices2D, GameObject gObj)
//	{
//
//		// Create the Vector3 vertices
//		Vector3[] vertices3D = new Vector3[3];
//		vertices3D [0] = new Vector3 (vertices2D[0].x, vertices2D[0].y, 0);
//		vertices3D [1] = new Vector3 (vertices2D[1].x, vertices2D[1].y, 0);
//		vertices3D [2] = new Vector3 (vertices2D[2].x, vertices2D[2].y, 0);
//		//			Debug.Log ("Vertices3D[0] " + vertices3D [0] + ", " + vertices3D [1] + ", " + vertices3D [2]);
//
//		// Use the triangulator to get indices for creating triangles
//		Triangulator tr = new Triangulator (vertices2D);
//		int[] indices = tr.Triangulate ();
//
//		// create polygons
//		MeshRenderer mRend = gObj.AddComponent<MeshRenderer>();
//		MeshFilter filter = gObj.AddComponent<MeshFilter> ();
//
//		mRend.material.color = new Color (1, 0, 0);
//		Mesh msh = new Mesh ();
//		msh.vertices = vertices3D;
//		msh.triangles = indices;
//		msh.RecalculateNormals ();
//		msh.RecalculateBounds ();
//
//		filter.mesh = msh;
//	}
//		
//}
