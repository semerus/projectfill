using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectionMenu_Stage : MonoBehaviour, IPointerClickHandler {
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		GameManager.loadLevel = transform.GetSiblingIndex();
		SceneManager.LoadScene ("room");
	}
	#endregion
}
