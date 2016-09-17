using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Gameplay_MenuList_SelectionMenu : MonoBehaviour, IPointerClickHandler {
	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		GuardManager.guardCount = 0;
		SceneManager.LoadScene ("StageSelect");
	}

	#endregion


}
