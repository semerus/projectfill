using System;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GiraffeStar;

public class WWWTopScore : MonoBehaviour {

	string url= "top_scores?GameId=1";

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
	IEnumerator Submit () {
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

			//ScoreSet ss = JsonManager.readScoreSet (w.text);
			//Debug.Log (w.text);
			//Debug.Log (this);

			var builder = new StringBuilder ();

			/*
			for (int i = 0; i < 3; i++) {
				builder.Append(ss.getScore ("High", (i+1)).ToString ());
			}
			for (int i = 0; i < 3; i++) {
				builder.Append(ss.getScore ("Low", (i+1)).ToString ());
			}
			*/
			log.text = w.text;
		}

	}
}