using System;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GiraffeStar;

public class WWWSubmitPlayResult : MonoBehaviour {
	string url;
	string submit_url= "/submit";

	void Start()
	{
		StartCoroutine ("Submit");
        url = Config.GetString("ServerEndpoint") + submit_url;
        Debug.Log(url);
	}

	// Use this for initialization
	IEnumerator Submit (int mapId, int userId, int numOfGuards, string guardLocation, string gameHash, float score) {
		Debug.Log ("Submit() of WWWSubmitPlayResult");

		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("MapId", mapId);
		form.AddField("UserId", userId);
		form.AddField("NumOfGuards", numOfGuards);
		form.AddField("GuardLocation", guardLocation);
		form.AddField("GameHash", gameHash);
		form.AddField("Score", "" + score);

		WWW w = new WWW(url, form);
		yield return w;

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
		} else {
			Debug.Log ("Finished sending PlayResult for MapId=" + mapId);
			Debug.Log(w.text);
		}
	}
}
