using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Submission_Return : MonoBehaviour, IPointerClickHandler {

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		SceneManager.LoadScene ("Room");
	}

	#endregion


}
