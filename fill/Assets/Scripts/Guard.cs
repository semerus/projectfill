using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class Guard : MonoBehaviour, IDragHandler{
	public GameObject vgMesh;
	public int layerMask = 1 << 8;
	Vector2[] mapVertices2D;

	// Setting Variables
	private const float angleDelta = 0.01f;
	private float RAY_DISTANCE = 100000f;
	private Color UNHIT_RAY_COLOR = Color.black;
	private Color GUARD_MODIFYING_COLOR = new Color(1, 0, 0, 0.3f);
	private Color GUARD_SET_COLOR = new Color(0, 0, 1, 0.3f);

	void Start () {
		layerMask = ~layerMask;
//		mapVertices2D = GameObject.Find("Map Generator").GetComponent<MapGenerator> ().vertices;
//		mapVertices2D = GetComponent<GameManager> ().getMapVertices2D ();

		/* Create GameObject to make VG */
		vgMesh = new GameObject ("VGMesher"); // VG stands for Visibility Graph
		vgMesh.transform.SetParent(transform);


		MeshRenderer mRend = vgMesh.AddComponent<MeshRenderer> ();
		mRend.material.color = GUARD_MODIFYING_COLOR;
		mRend.material.shader = Shader.Find("Transparent/Diffuse");

		MeshFilter filter = vgMesh.AddComponent<MeshFilter> () as MeshFilter;
		filter.mesh = new Mesh ();

		HashSet<Vector2> unorderedVertices = ShootRays (gameObject.transform.position, layerMask, GameManager.getInstance().getMapData()); 
		Vector2[] toArray = unorderedVertices.ToArray ();
		Array.Sort (toArray, new ClockwiseVector2Comparer (gameObject.transform.position));
		renderVG (toArray);
	}

	void LateUpdate () {
		vgMesh.transform.position = Vector3.zero;
	}

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		Vector3 nextPos;
		nextPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
		transform.position = nextPos;

		// retrieve unorderedVertices by shooting rays to vertex of map
		HashSet<Vector2> unorderedVertices = ShootRays (gameObject.transform.position, layerMask, GameManager.getInstance().getMapData());

		// sort the unorderedVertices
		Vector2[] toArray = unorderedVertices.ToArray ();
		Array.Sort (toArray, new ClockwiseVector2Comparer (gameObject.transform.position));

		// renderVG
		renderVG (toArray);
	}
	#endregion


	/**
	 * For every vertex in the map,
	 * 1. Calculate Angles
	 * 2. Shoot rays with the calculated angles
	 * 3. Add hit point to unorderedVertices if the ray hits collider
	*/
	HashSet<Vector2> ShootRays (Vector3 targetPos, int layerMask, MapData md) {
		HashSet<Vector2> unorderedVertices = new HashSet<Vector2>();

		// add targetPos to unorderVertices if targetPos is outside polygon
		// unorderedVertices.Add (targetPos);
		Vector2[] outer = md.getOuter().getVertices();
		SimplePolygon2D[] holes = md.getHoles();

		int totalCount = outer.Length;
		for(int i = 0; i < holes.Length; i++){
			totalCount += holes [i].getVertices ().Length;
		}

		Vector2[] mapVertices2D = new Vector2[totalCount];
		int currentIndex = 0;
		for (int i = 0; i < outer.Length; i++) {
			mapVertices2D [currentIndex++] = outer [i];
		}

		for (int i = 0; i < holes.Length; i++) {
			for (int j = 0; j < holes [i].getVertices ().Length; j++) {
				mapVertices2D [currentIndex++] = holes [i].getVertices () [j];
			}
		}

		const int maxCount = 10000; // max angle turn

		for (int i = 0; i < mapVertices2D.Length - 1; i++) {

			/* 1. Calculate Angles */

			// Calculate standardAngle
			Vector2 standardAngle = (mapVertices2D [i] - new Vector2(targetPos.x, targetPos.y));

			// Calculate leftAngle
			Vector2 leftAngle = standardAngle;
			int count = 0;
			do{
				leftAngle = Quaternion.AngleAxis (angleDelta, Vector3.forward) * leftAngle;
				count++;
			} while (leftAngle == standardAngle && count < maxCount);

			// Calculate rightAngle
			Vector2 rightAngle = standardAngle;
			count = 0;
			do {
				rightAngle = Quaternion.AngleAxis (-angleDelta, Vector3.forward) * rightAngle;
				count++;
			} while (rightAngle == standardAngle && count < maxCount);

			/* 2. Shoot rays with the calculated angles */
			// Color : Left Blue, Right White
			RaycastHit2D rightHit = raycastWithDebug (targetPos, rightAngle, Color.white, layerMask);
			RaycastHit2D leftHit = raycastWithDebug (targetPos, leftAngle, Color.blue, layerMask);
			RaycastHit2D hit = raycastWithDebug (targetPos, standardAngle, Color.red, layerMask);

			/* 3. Add hit point to unorderedVertices if the ray hits collider */
			// check if the ray is hit
			// add to the unorderedVertices only if the ray is hit
			if (leftHit.collider != null) {	unorderedVertices.Add (leftHit.point); }
			if (hit.collider != null) { unorderedVertices.Add (hit.point); }
			if (rightHit.collider != null) { unorderedVertices.Add (rightHit.point); }
		}

		// Note.
		// Debug.Assert (addCounted == unorderVertices.ToArray().Length);
		// By inspecting the above Assert, which happens when the guard in on the line of Collider,
		// we should decide wheter to allow guard on edge or not
		return unorderedVertices;
	}

	/**	
	 * rayFrom : Vector3 
	 * 		Position where the ray starts from.
	 * shootAtAngle : Vector3
	 * 		Angle (or direction in 3D vector) to indicate the direction to shoot the ray
	 * lineColorToHitPosition : Color
	 * 		Color of line which hits the collider. If no collider is hit, draw ray with
	 * 		the color of the variable UNHIT_RAY_COLOR
	 * layerMask :  int
	 * 		Information which layer should the ray ignore
	 * 
	 * Return Value:
	 * 		Return the result of Physics2D.Raycast
	 * 		The result is never null, instead check if the [Return Value].collider is null.
	 */
	RaycastHit2D raycastWithoutDebug (Vector3 rayFrom, Vector3 shootAtAngle, int layerMask)
	{
		RaycastHit2D hitPosition = Physics2D.Raycast (rayFrom, shootAtAngle, RAY_DISTANCE, layerMask);

		return hitPosition;
	}

	/**	
	 * rayFrom : Vector3 
	 * 		Position where the ray starts from.
	 * shootAtAngle : Vector3
	 * 		Angle (or direction in 3D vector) to indicate the direction to shoot the ray
	 * lineColorToHitPosition : Color
	 * 		Color of line which hits the collider. If no collider is hit, draw ray with
	 * 		the color of the variable UNHIT_RAY_COLOR
	 * layerMask :  int
	 * 		Information which layer should the ray ignore
	 * 
	 * Return Value:
	 * 		Return the result of Physics2D.Raycast
	 * 		The result is never null, instead check if the [Return Value].collider is null.
	 */
	RaycastHit2D raycastWithDebug (Vector3 rayFrom, Vector3 shootAtAngle, Color lineColorToHitPosition, int layerMask)
	{
		RaycastHit2D hitPosition = Physics2D.Raycast (rayFrom, shootAtAngle, RAY_DISTANCE, layerMask);

		if (hitPosition.collider != null)
			Debug.DrawLine (rayFrom, hitPosition.point, lineColorToHitPosition);
		else {
			Debug.DrawRay (rayFrom, shootAtAngle, UNHIT_RAY_COLOR);
		}
		return hitPosition;
	}

	/**
	 * vertices2D : Vector2[]
	 * 		Vertices to draw a polygon
	 * 
	 * This function sets the "mesh" of vgMesh : GameObject.
	 */
	void renderVG(Vector2[] vertices2D)
	{
		// Create the Vector3 vertices
		Vector3[] vertices3D = new Vector3[vertices2D.Length];
		for (int i = 0; i < vertices2D.Length; i++) {
			vertices3D [i] = new Vector3 (vertices2D [i].x, vertices2D [i].y, 0);
		}

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator (vertices2D);
		int[] indices = tr.Triangulate ();

		// create polygons
		Mesh msh = vgMesh.GetComponent<MeshFilter> ().mesh;
		msh.Clear ();
		msh.vertices = vertices3D;
		msh.triangles = indices;
		msh.RecalculateNormals ();
		msh.RecalculateBounds ();
	}

	/**
	 * This class is a IComparer<Vector2>.
	 * When initializing the Comparer, send rayFrom : Vector2.	
	 */
	private class ClockwiseVector2Comparer : IComparer<Vector2>
	{
		private Vector2 rayFrom;

		public ClockwiseVector2Comparer(Vector2 respectTo){
			this.rayFrom = respectTo;
		}

		public int Compare(Vector2 v1, Vector2 v2)
		{
			// use cross product to calculate area
			float area2 = area (rayFrom, v1, v2); 

			if (area2 > 0) {
				return 1;
			} else if (area2 == 0) {
				// distance used for tie breaker
				float distComp = Vector2.Distance (rayFrom, v1) - Vector2.Distance (rayFrom, v2);
				if (distComp > 0) {
					return 1;
				} else if (distComp < 0) {
					return -1;
				} else {
					return 0;
				}
			} else {
				return -1;
			}
		}

		// cross product function which calculates the area bounded by 3 vertices
		private float area(Vector2 a, Vector2 b, Vector2 c){
			// i.e. draw ray from a to b
			// if c is left of the extended line from a to b, then it returns a positive value
			return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
		}
	}

/*
	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

//		GuardManager.setSelectedGuard (gameObject);

		gameObject.GetComponent<Renderer> ().material.color = GUARD_MODIFYING_COLOR;

		Debug.Log (offset);
	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

		if(GuardManager.JudgeBounds(curPosition))
			gameObject.transform.position = curPosition;

		// retrieve unorderedVertices by shooting rays to vertex of map
		HashSet<Vector2> unorderedVertices = ShootRays (gameObject.transform.position, layerMask, GameManager.getInstance().getMapData());

		// sort the unorderedVertices
		Vector2[] toArray = unorderedVertices.ToArray ();
		Array.Sort (toArray, new ClockwiseVector2Comparer (gameObject.transform.position));

		// renderVG
		renderVG (toArray);
	}

	void OnMouseUp()
	{
		gameObject.GetComponent<Renderer> ().material.color = GUARD_SET_COLOR;
		Debug.Log (gameObject.transform.position);


	}
*/
}
