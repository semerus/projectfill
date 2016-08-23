using UnityEngine;
using System.Collections;

public class GuardManager : MonoBehaviour {
	public GameObject guardPrefab;
	public int guardCount = 0;

	private GameObject selected;

	// Setting Variables
	private const float distance = 30f; // TODO: Rename
	private const float maxX = 10, maxY = 10, minX = -10, minY = -10;

	void Start () {
	}

	void Update () {
		ControlGuard ();
	}

	/**
	 * Composition of all controls of the Guard Object
	 * Creation, Movement, Deletion included
	 */

	void ControlGuard() {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
			if (hitInfo.collider == null)
				CreateGuard ();
			else if (hitInfo.transform.gameObject.tag != "Guard")
				CreateGuard ();
			else if (!Input.GetMouseButtonUp (0)){
				Vector3 mousePos = Input.mousePosition;
				selected = hitInfo.collider.gameObject;
				selected.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, distance));
				selected.GetComponent<Guard> ().change = true;
			}
		}
		
	}
	void CreateGuard() {
			GameObject guard = Instantiate (guardPrefab) as GameObject;
			guard.name = "Guard" + ++guardCount;
			guard.transform.position = PositionGuard ();
	}

	void MoveGuard(RaycastHit2D hitInfo) {
		Transform transform = hitInfo.collider.GetComponentInParent<Transform> ();
		transform.position = PositionGuard ();
	}

	void DeleteGuard() {
		
	}

	/**
	 * Checks position of the mouse and returns whether Guard Object all ready exists(true) or not(false)
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
