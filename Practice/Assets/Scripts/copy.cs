using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class copy : MonoBehaviour, IPointerClickHandler, IDropHandler {

	bool isRed;

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		isRed = !isRed;
		if (isRed) {
			GetComponent<Image> ().color = Color.red;
		} else {
			GetComponent<Image> ().color = Color.white;
		}
	}

	#endregion

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		Destroy (eventData.pointerDrag);
	}

	#endregion



}
