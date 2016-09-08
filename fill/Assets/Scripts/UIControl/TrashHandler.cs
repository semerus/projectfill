using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TrashHandler : MonoBehaviour, IDropHandler {
	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		Destroy (eventData.pointerDrag);
		GuardManager.guardCount--;
	}

	#endregion
}
