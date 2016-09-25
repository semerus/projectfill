using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectionMenu_Swipe : MonoBehaviour {
	private Vector3 startPos;
	private float startTime;
	private bool onSwipe = false;

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			startPos = Input.mousePosition;
			onSwipe = true;
			startTime = Time.time;
		}
		if (Input.GetMouseButton(0)) {
			if (Mathf.Abs(Input.mousePosition.x - startPos.x) > 200f)
				onSwipe = false;
		}
		if (Input.GetMouseButtonUp(0)) {
			if (Time.time - startTime > 2f)
				onSwipe = false;
			if (Mathf.Abs(Input.mousePosition.y - startPos.y) < 60f)
				onSwipe = false;
			if (onSwipe == true) {
				if (Input.mousePosition.y - startPos.y > 0) {
					Debug.Log("Swipe Sucessful Up");
				} else if (Input.mousePosition.y - startPos.y < 0) {
					SceneManager.LoadScene(2);
					Debug.Log("Swipe Sucessful Down");
				}
				onSwipe = false;
			} else {
				Debug.Log("Swipe Fail");
			}
		}

		#endif

		#if UNITY_ANDROID

		#endif
	}
}
