using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour, IPointerClickHandler, IDragHandler {

	bool isRed;

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		isRed = !isRed;
		if (isRed) {
			GetComponent<SpriteRenderer> ().color = Color.red;
		} else {
			GetComponent<SpriteRenderer> ().color = Color.white;
		}
		Debug.Log ("click");
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		Vector3 nextPos;
		nextPos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
		transform.position = nextPos;
	}

	#endregion
	
}
