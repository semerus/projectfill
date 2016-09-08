using UnityEngine;
using System.Collections;

public class GuardManager : MonoBehaviour {
	public GameObject guardPrefab;
	public static int guardCount = 0;

	public static GuardManager instance = null;
	private static GameObject selectedGuard;

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
//				Debug.Log ("Nothing hit, creating new guard");
				CreateGuard ();
			}
//			} else if (hitInfo.collider.tag != "Guard"){
//				// do nothing
//				Debug.Log ("Hit but not guard, do nothing");
//			} else if (selectedGuard != null) {
//				Debug.Log ("SelectedGuard is not null");
//				MoveGuard(hitInfo);
//			}
//			else {
//				Debug.Log ("Else, selectedGuard set");
//				selectedGuard = hitInfo.collider.gameObject;
//			}
		}
	}

	void CreateGuard() {
//		GameObject guard = Instantiate (guardPrefab) as GameObject;
		GameObject guard = Instantiate (Resources.Load("Vertex") as GameObject);
		guard.name = "Guard" + ++guardCount;
		guard.transform.position = PositionGuard ();
//		selectedGuard = guard;
	}

	/**
	 * Checks position of the mouse and returns whether Guard Object already exists(true) or not(false)
	 */
	/*
	GameObject CheckGameobject() {
		RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
		if (hitInfo.transform.gameObject != null)
			return hitInfo.transform.gameObject;
		else
			return new GameObject();
	}
	*/

	/**
	 * Get the position of the mouse and set the position of Guard Object
	 */
	Vector3 PositionGuard () {
		Vector3 mousePos = Input.mousePosition;
		Vector3 targetPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, distance));

		if (JudgeBounds (targetPos)) {
			return targetPos;
		} else {
			Debug.LogError ("Outside Bounds");
			return new Vector3 ();
		}
	}

	bool JudgeBounds (Vector3 pos) {
		if (!(pos.x > maxX || pos.y > maxY || pos.x < minX || pos.y < minY))
			return true;
		else
			return false;
	}
}
