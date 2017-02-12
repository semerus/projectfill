using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionMenu_StageScroll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float cellWidth = GetComponent<GridLayoutGroup> ().cellSize.x;
		float padding = GetComponent<GridLayoutGroup> ().padding.left;
		float spacing = GetComponent<GridLayoutGroup> ().spacing.x;
		int cellNum = transform.childCount;
		float temp = cellWidth * cellNum + 2 * padding + spacing * (cellNum - 1);
		RectTransform rt = GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (temp, 900f);
		rt.anchoredPosition = new Vector2 (1490f, 0f);
	}
}
