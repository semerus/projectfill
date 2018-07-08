using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GiraffeStar;

public class ServerConnection : MonoBehaviour {

	string url;

	void Start()
	{
		url = "http://ec2-52-78-152-38.ap-northeast-2.compute.amazonaws.com/";
		StartCoroutine ("Submit");
		Debug.Log(url);
	}

	// Use this for initialization
	IEnumerator Submit () {
		Debug.Log ("Start() of WWWTopScore");

		WWW w = new WWW (url);
		yield return w;
		Debug.Log ("Returned");

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
			Debug.Log ("Error");
		} else {
			Debug.Log ("Finished downloading message" + w.text);
			new ServerResultMessage () {
				isSuccess = true,
			}.Dispatch ();
		}

	}
}

public class ServerResultMessage : MessageCore
{
	public bool isSuccess;
}