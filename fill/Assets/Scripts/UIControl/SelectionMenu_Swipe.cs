using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenu_Swipe : MonoBehaviour {

	public GameObject stagePanel;
	public GameObject themePanel;

	private Vector3 startPos;
	private float startTime;
	private bool onSwipe = false;
	private bool movePanel = false;

	// Update is called once per frame
	void Update () {
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
					themePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector2(0f, 1080f);
					Debug.Log("Swipe Sucessful Up");
				} else if (Input.mousePosition.y - startPos.y < 0) {
					//SceneManager.LoadScene(2);
					themePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector2(0f, 0f);
//					Vector2.MoveTowards(themePanel.GetComponent<RectTransform> ().anchoredPosition, new Vector2(0f, 0f), 1f * Time.deltaTime);
//					Vector2.MoveTowards(stagePanel.GetComponent<RectTransform> ().anchoredPosition, new Vector2(0f, -1080f), 500f);
					movePanel = true;
					Debug.Log("Swipe Sucessful Down");
				}
				onSwipe = false;
			} else {
				Debug.Log("Swipe Fail");
			}
		}
		if (movePanel) {
			AnimatePanel (stagePanel, new Vector2 (0f, -1080f), 10f);
		}
	}

	private void AnimatePanel(GameObject panel, Vector2 des, float speed) {
		panel.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0f, speed);
		if (panel.GetComponent<RectTransform> ().anchoredPosition == des) {
			movePanel = false;
			return;
		}
	}
}


