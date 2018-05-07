using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WWWTopScore : MonoBehaviour {

	public string url= "http://127.0.0.1:8080/top_scores";

	public Text[] highest = new Text[3];
	public Text[] lowest = new Text[3];

	// Use this for initialization
	IEnumerator Start () {
		Debug.Log ("Start() of WWWTopScore");

		// Create a Web Form
		//		WWWForm form = new WWWForm();
		//		form.AddField("GameId", GameManager.MapData.getId());
		//		form.AddField("Email", SystemInfo.deviceUniqueIdentifier);

		// Upload to a cgi script
		//		WWW w = new WWW(url, form);
		WWW w = new WWW (url);
		yield return w;
		Debug.Log ("Returned");

		if (!string.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
			Debug.Log ("Error");
		} else {
			Debug.Log ("Finished downloading topscore");

			ScoreSet ss = JsonManager.readScoreSet (w.text);
			Debug.Log (w.text);
			Debug.Log (this);
			for (int i = 0; i < 3; i++) {
				highest[i].text = ss.getScore ("High", (i+1)).ToString ();
			}
			for (int i = 0; i < 3; i++) {
				lowest[i].text = ss.getScore ("Low", (i+1)).ToString ();
			}
		}

	}
}