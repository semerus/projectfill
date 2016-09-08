using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gameplay_MenuClick : MonoBehaviour, IPointerClickHandler {

//	this script opens and closes the menu
	GameObject menuList;

	void Awake () {
		menuList = GameObject.Find ("MenuList");
		menuList.SetActive (false);
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		if (!menuList.activeSelf)
			menuList.SetActive (true);
		else
			menuList.SetActive (false);
	}

	#endregion


}
