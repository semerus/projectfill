﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gameplay_Submit : MonoBehaviour, IPointerClickHandler {

	void OnEnable() {
		Color temp = new Color();
		temp.a = 1f;
		gameObject.GetComponent<Image> ().color = temp;
	}

	void OnDisable() {
		Color temp = new Color();
		temp.a = 0.3f;
		gameObject.GetComponent<Image> ().color = temp;
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		SceneManager.LoadScene ("Submission", LoadSceneMode.Additive);
//		var sample = new GameObject ();
//		sample.AddComponent (typeof(WWWTopScore));
//		sample.AddComponent (typeof(RESTTest));
//		sample.AddComponent (typeof(GUIText));
//		sample.AddComponent (typeof(GUIText));
	}

	#endregion
}
