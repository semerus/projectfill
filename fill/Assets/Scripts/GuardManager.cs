using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardManager : MonoBehaviour {
	private static GuardManager instance = null;
	public static int guardCount = 0; //number of current guards
	private static int guardIdCount = 0; //generates guardId for each Guard
	private static Stack<HistoryData> historyList = new Stack<HistoryData>(); //stack storing action history data
	private static Dictionary<int, Guard> guardDic = new Dictionary<int, Guard>(); //dictionary storing with guardId as key, Guard as value
	public static List<Guard> guardList = new List<Guard>();

//	private const float maxX = 10, maxY = 10, minX = -10, minY = -10;

	/*****************************************************************/
	/*Getters and Setters*/
	public static GuardManager Instance {
		get {
			return instance;
		}
	}

	public static int GuardIdCount {
		get {
			return guardIdCount;
		}
		set {
			guardIdCount = value;
		}
	}

	public static Stack<HistoryData> HistoryList {
		get {
			return historyList;
		}
	}

	public static Dictionary<int, Guard> GuardDic {
		get {
			return guardDic;
		}
	}

	/*****************************************************************/
	/*MonoBehaviour*/
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (gameObject);
	}

	void Start () {
		historyList.Push (new HistoryData (HistoryState.Start));
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (Camera.main.ScreenPointToRay (Input.mousePosition));
			if (hitInfo.collider == null) {
				CreateGuard ();
			}
		}

		bool filled = DecisionAlgorithm.isFilled (GuardManager.getPositionList (), GameManager.getMapData());
		if (filled)
			GameObject.Find ("Submit").GetComponent<Gameplay_Submit> ().enabled = true;
			//Debug.Log ("Filled");
		else
			GameObject.Find ("Submit").GetComponent<Gameplay_Submit> ().enabled = false;
			//Debug.Log ("Not Filled");
	}

	/*****************************************************************/
	/*Functions*/
	public void CreateGuard() {
		if (JudgeBounds (PositionGuard ())) {
			GameObject guard = Instantiate (Resources.Load ("Vertex") as GameObject);
			guard.name = "Guard" + ++guardCount;
			guard.transform.position = PositionGuard ();
		}
	}

	public void CreateGuardForReverse(Vector3 pos, int guardId) {
		GameObject guard = Instantiate (Resources.Load ("Vertex") as GameObject);
		guard.name = "Guard" + ++guardCount;
		guard.transform.position = pos;
		guard.GetComponent<Guard> ().GuardId = guardId;
	}

	/**
	 * Get the position of the mouse and set the position of Guard Object
	 */
	Vector3 PositionGuard () {
		Vector3 mousePos = Input.mousePosition;
		Vector3 targetPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 
			-Camera.main.transform.position.z));
		return targetPos;
	}

	public static bool JudgeBounds (Vector3 pos) {
//		if (!(pos.x > maxX || pos.y > maxY || pos.x < minX || pos.y < minY))
//			return true;
//		else
//			return false;
		return isValidPosition(pos, GameManager.getMapData());
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
