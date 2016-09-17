using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionMenu_Stage : MonoBehaviour, IPointerClickHandler {

	void Start () {
		if (transform.GetSiblingIndex () < GameManager.getInstance ().getMapList ().Count) {
			GetComponentInChildren<Text> ().text = GameManager.getInstance ().getMapList () [transform.GetSiblingIndex ()].Name;
			transform.GetChild(0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (GameManager.getInstance ().getMapList () [transform.GetSiblingIndex ()].ImgPath);
			Debug.Log (GameManager.getInstance ().getMapList () [transform.GetSiblingIndex ()].ImgPath);
		}
		else {
			GetComponentInChildren<Text> ().text = "NoData";
			Debug.LogError ("No data in JSON file");
		}
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		if (!(transform.GetSiblingIndex () < GameManager.getInstance ().getMapList ().Count)) {
			Debug.LogError ("No data in JSON file");
			return;
		}
			
		GameManager.loadLevel = transform.GetSiblingIndex();
		SceneManager.LoadScene ("room");
	}
	#endregion
}
