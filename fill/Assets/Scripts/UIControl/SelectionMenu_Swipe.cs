using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenu_Swipe : MonoBehaviour {

	public GameObject stagePanel;
	public GameObject themePanel;
	public float speed;

	private Vector3 startPos;
	private float startTime;
	private bool onSwipe = false;
	private bool movePanel = false;
	private Vector2 stageDesPos; // panel movement destination(for each panel)
	private Vector2 themeDesPos;
	private int moveDir; // directions (+1/-1)

	void Update () {
		/***** UnityEditor *****/
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
				if (Input.mousePosition.y - startPos.y > 0) { //swipe up
					themeDesPos = new Vector2 (0f, 1080f);
					stageDesPos = new Vector2 (0f, 0f);
					moveDir = 1;
					movePanel = true;
					Debug.Log("Swipe Sucessful Up");
				} else if (Input.mousePosition.y - startPos.y < 0) { //swipe down
					themeDesPos = new Vector2 (0f, 	0f);
					stageDesPos = new Vector2 (0f, -1080f);
					moveDir = -1;
					movePanel = true;
					Debug.Log("Swipe Sucessful Down");
				}
				onSwipe = false;
			} else {
				Debug.Log("Swipe Fail");
			}
		}

		/***** Touchscreen *****/
		#elif UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			startPos = Input.GetTouch (0).position;
			onSwipe = true;
			startTime = Time.time;
		}
		if (Input.GetTouch (0).phase == TouchPhase.Moved) {
			if (Mathf.Abs(Input.GetTouch(0).position.x - startPos.x) > 200f)
				onSwipe = false;
		}
		if (Input.GetTouch(0).phase == TouchPhase.Ended) {
			if (Time.time - startTime > 2f)
				onSwipe = false;
			if (Mathf.Abs(Input.GetTouch(0).position.y - startPos.y) < 60f)
				onSwipe = false;
			if (onSwipe == true) {
				if (Input.GetTouch(0).position.y - startPos.y > 0) { //swipe up
					themeDesPos = new Vector2 (0f, 1080f);
					stageDesPos = new Vector2 (0f, 0f);
					moveDir = 1;
					movePanel = true;
					Debug.Log("Swipe Sucessful Up");
				} else if (Input.GetTouch(0).position.y - startPos.y < 0) { //swipe down
					themeDesPos = new Vector2 (0f, 	0f);
					stageDesPos = new Vector2 (0f, -1080f);
					moveDir = -1;
					movePanel = true;
					Debug.Log("Swipe Sucessful Down");
				}
				onSwipe = false;
			} else {
				Debug.Log("Swipe Fail");
			}
		}
		#endif

		/***** Panel Animation *****/
		if (movePanel) {
			AnimatePanel (stagePanel, stageDesPos, speed * moveDir);
			AnimatePanel (themePanel, themeDesPos, speed * moveDir);
		}
	}

	private void AnimatePanel(GameObject panel, Vector2 des, float speed) {
		if (Mathf.Abs(panel.GetComponent<RectTransform> ().anchoredPosition.y - des.y) <= Mathf.Abs(speed))
			panel.GetComponent<RectTransform> ().anchoredPosition = des;
		else 
			panel.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0f, speed);
		if (panel.GetComponent<RectTransform> ().anchoredPosition == des) {
			movePanel = false;
			return;
		}
	}
}

