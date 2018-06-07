//using UnityEngine;
//using System.Collections;
//using UnityEngine.EventSystems;

//public class Gameplay_Trash : MonoBehaviour, IDropHandler {
//	#region IDropHandler implementation

//	public void OnDrop (PointerEventData eventData)
//	{
//		GuardManager.HistoryList.Push (new HistoryData(HistoryState.Create, eventData.pointerDrag.GetComponent<Guard>().GuardId, eventData.pointerDrag.GetComponent<Guard>().PreviousPos));
//		Destroy (eventData.pointerDrag);
//	}

//	#endregion
//}
