using System;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GiraffeStar;

public class WWWSubmitMap : MonoBehaviour {
	string url;
	string submit_url= "/map";

	void Start()
	{
		StartCoroutine ("Submit");
        url = Config.GetString("ServerEndpoint") + submit_url;
        Debug.Log(url);
	}

	// Use this for initialization
	IEnumerator Submit (string mapHash, string mapFile) {
		Debug.Log ("Submit() of WWWSubmitMap");

		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("MapHash", mapHash);
		form.AddField("MapFile", mapFile);

		WWW w = new WWW(url, form);
		yield return w;

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
		} else {
			Debug.Log ("Finished sending MapData");
			Debug.Log(w.text);
		}
	}
}
