using System;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GiraffeStar;

public class WWWGetMap : MonoBehaviour {
	string url;
	string submit_url= "/map?MapId=";

	void Start()
	{
		StartCoroutine ("Submit");
        url = Config.GetString("ServerEndpoint") + submit_url;
        Debug.Log(url);
	}

	// Use this for initialization
	IEnumerator Submit (int mapId) {
		Debug.Log ("Submit() of WWWGetMap");

		WWW w = new WWW(url + mapId);
		yield return w;

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
		} else {
			Debug.Log ("Finished querying MapData for MapId=" + mapId);
			Debug.Log (w.text);
		}
	}
}
