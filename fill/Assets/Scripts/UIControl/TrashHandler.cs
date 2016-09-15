using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TrashHandler : MonoBehaviour, IDropHandler {
	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		GuardManager.guardList.Remove (eventData.pointerDrag.GetComponent<Guard> ());
		Destroy (eventData.pointerDrag);
		GuardManager.guardCount--;
	}

	#endregion
}
