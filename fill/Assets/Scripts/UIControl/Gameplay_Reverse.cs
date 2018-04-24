using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Gameplay_Reverse : MonoBehaviour, IPointerClickHandler {
	
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		HistoryData hd = GuardManager.HistoryList.Pop ();
		switch (hd.State) {
		case HistoryState.Create:
			{
				GuardManager.Instance().CreateGuardForReverse (hd.Position, hd.GuardId);
				break;
			}
		case HistoryState.Destroy:
			{
				Guard temp;
				bool test;
				test = GuardManager.GuardDic.TryGetValue (hd.GuardId, out temp);
				Debug.Log ("destroy: " + test);
				Destroy (temp.gameObject);
				break;
			}
		case HistoryState.Move:
			{
				Guard temp;
				bool test;
				test = GuardManager.GuardDic.TryGetValue (hd.GuardId, out temp);
				Debug.Log ("move: " + test);
				temp.transform.position = hd.Position;
				break;
			}
		case HistoryState.Start:
			GuardManager.HistoryList.Push (new HistoryData (HistoryState.Start));
			break;
		}
	}
	#endregion
}
