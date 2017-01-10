using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubmissionMenu_Continue : MonoBehaviour, IPointerClickHandler {

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		SceneManager.LoadScene ("room");
		ServerConnection server = new ServerConnection ();
		server.SendGameInfo ();
		server.ReceiveScoreInfo ();
	}

	#endregion


}
