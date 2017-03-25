using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Gameplay_MenuList_ResetMenu : MonoBehaviour, IPointerClickHandler {
	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		SceneManager.LoadScene ("Room");
	}

	#endregion
}