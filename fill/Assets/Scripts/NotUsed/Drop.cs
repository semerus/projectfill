using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrop (PointerEventData eventData) {
			Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

			//DragHandling dragHandling = eventData.pointerDrag.GetComponent<DragHandling>();
	}
}
