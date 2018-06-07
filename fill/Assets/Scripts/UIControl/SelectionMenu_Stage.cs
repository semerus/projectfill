//using UnityEngine;
//using System.Collections;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class SelectionMenu_Stage : MonoBehaviour, IPointerClickHandler {

//	void Start () {
//		if (transform.GetSiblingIndex () < GameManager.MapList.Count) {
//			GetComponentInChildren<Text> ().text = GameManager.MapList [transform.GetSiblingIndex ()].Name;
//			transform.GetChild(0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (GameManager.MapList [transform.GetSiblingIndex ()].ImgPath);
//			Debug.Log (GameManager.MapList [transform.GetSiblingIndex ()].ImgPath);
//		}
//		else {
//			GetComponentInChildren<Text> ().text = "NoData";
//			Debug.LogError ("No data in JSON file");
//		}
//	}

//	#region IPointerClickHandler implementation
//	public void OnPointerClick (PointerEventData eventData)
//	{
//		if (!(transform.GetSiblingIndex () < GameManager.MapList.Count)) {
//			Debug.LogError ("No data in JSON file");
//			return;
//		}
			
//		GameManager.loadLevel = transform.GetSiblingIndex();
//		SceneManager.LoadScene ("Room");
//	}
//	#endregion
//}
