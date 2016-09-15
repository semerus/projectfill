using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardManager : MonoBehaviour {
	public static int guardCount = 0;

	public static GuardManager instance = null;
	public static List<Guard> guardList = new List<Guard>();

//	foreach (KeyValuePair<int, Guard> kv in GuardManager.guardDic) {
//		Debug.Log("Key: " + kv.Key + "Pos: " + kv.Value.transform.position);
//	}

	// Setting Variables
	private const float distance = 30f; // TODO: Rename
	private const float maxX = 10, maxY = 10, minX = -10, minY = -10;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (gameObject);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (Camera.main.ScreenPointToRay (Input.mousePosition));
			if (hitInfo.collider == null) {
				CreateGuard ();
			}
		}
	}

	/*****************************************************************/
	void CreateGuard() {
		if (JudgeBounds (PositionGuard ())) {
			GameObject guard = Instantiate (Resources.Load ("Vertex") as GameObject);
			guard.name = "Guard" + ++guardCount;
			guard.transform.position = PositionGuard ();
		}
	}

	/**
	 * Get the position of the mouse and set the position of Guard Object
	 */
	Vector3 PositionGuard () {
		Vector3 mousePos = Input.mousePosition;
		Vector3 targetPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, distance));
		return targetPos;
	}

	public static bool JudgeBounds (Vector3 pos) {
//		if (!(pos.x > maxX || pos.y > maxY || pos.x < minX || pos.y < minY))
//			return true;
//		else
//			return false;
		return isValidPosition(pos, GameManager.getInstance().getMapData());
	}

	static bool isValidPosition(Vector3 position, MapData md){
		//guard has to be inside outer, but outside holes

		// first check if the position is inside outer
		if (!md.getOuter ().isInsidePolygon (position)) {
			return false;
		}

		// check if the position is outside of every hole
		SimplePolygon2D[] holes = md.getHoles();
		for (int i = 0; i < holes.Length; i++) {
			if (holes [i].isInsidePolygon (position))
				return false;
		}

		return true;
	}

	// returns array of current guard positions
	public static Vector3[] getPositionList () {
		Vector3[] list = new Vector3[guardList.Count];
		for (int i = 0; i < guardList.Count; i++) {
			list [i] = guardList [i].transform.position;
		}
		return list;
	}
}
