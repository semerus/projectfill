using System;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GiraffeStar;

public class WWWAllScores : MonoBehaviour {

	string url= "all_scores?MapId=";

	//public Text[] highest = new Text[3];
	//public Text[] lowest = new Text[3];

	public Text log;

	void Start()
	{
		StartCoroutine ("Submit");
        url = Config.GetString("ServerEndpoint") + url;
        Debug.Log(url);
	}

	// Use this for initialization
	IEnumerator Submit (int mapId) {
		Debug.Log ("Start() of WWWAllScores");

		WWW w = new WWW (url + mapId);
		yield return w;

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
		} else {
			Debug.Log ("Finished downloading all scores for MapId=" + mapId);
			Debug.Log(w.text);
		}
	}

	// overloaded method
	IEnumerator Submit (int mapId, int N) {
		Debug.Log ("Start() of WWWTopScore");

		WWW w = new WWW (url + mapId + "&N=" + N);
		yield return w;

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
		} else {
			Debug.Log ("Finished downloading topscore for MapId=" + mapId + " & N=" + N);
			Debug.Log(w.text);
		}
	}
}
